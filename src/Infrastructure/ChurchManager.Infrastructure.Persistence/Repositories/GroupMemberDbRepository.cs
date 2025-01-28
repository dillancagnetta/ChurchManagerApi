#region

using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class GroupMemberDbRepository: GenericRepositoryBase<GroupMember>, IGroupMemberDbRepository
{
    public GroupMemberDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
    {
    }

    /// <summary>
    /// Returns a queryable collection of <see cref="GroupMember">GroupMembers</see> who are members of a specific group.
    /// </summary>
    public IQueryable<GroupMember> GetByGroupId(int groupId)
    {
        return Queryable("Person", "GroupRole")
            .Where(t => t.GroupId == groupId)
            .OrderBy(g => g.GroupRole.Name);
    }

    /// <summary>
    /// Gets the active leaders of the group
    /// </summary>
    public IQueryable<GroupMember> GetLeaders(int groupId)
    {
        return GetByGroupId(groupId)
            .AsNoTracking()
            .Where(GroupMemberCriteria.IsLeader);
    }


    public async Task<(int peopleCount, int leadersCount)> PeopleAndLeadersInGroupsAsync(int groupTypeId, CancellationToken ct = default)
    {
        var query = Queryable("GroupRole", "Group")
            .Where(x => x.Group.GroupTypeId == groupTypeId);

        var peopleCount = await query
            .CountAsync(GroupMemberCriteria.IsNotLeaderFilter, cancellationToken: ct);

        // Extra strict requirements as Cell Assistants are also marked leaders
        var leadersCount = await query
            .CountAsync(GroupMemberCriteria.IsLeaderOrCanManageFilter, cancellationToken: ct);

        return (peopleCount, leadersCount);
    }

    public async Task<(int peopleCount, int leadersCount)> PeopleAndLeadersInGroupAsync(int groupId, CancellationToken ct = default)
    {
        var query = Queryable("GroupRole")
            .Where(x => x.GroupId == groupId);

        var peopleCount = await query
            .CountAsync(GroupMemberCriteria.IsNotLeaderFilter, cancellationToken: ct);

        // Extra strict requirements as Cell Assistants are also marked leaders
        var leadersCount = await query
            .CountAsync(GroupMemberCriteria.IsLeaderOrCanManageFilter, cancellationToken: ct);

        return (peopleCount, leadersCount);
    }

    public async Task AddGroupMember(int groupId, int personId, int groupRoleId, DateTime? firstVisitDate = null,
        string communicationPreference = "Email", CancellationToken ct = default)
    {
        // Check they are not a group member already
        if (await Queryable().AnyAsync(m =>
                m.PersonId == personId && m.GroupId == groupId, ct))
            return;

        var groupMember = new GroupMember
        {
            PersonId = personId,
            GroupId = groupId,
            GroupRoleId = groupRoleId,
            FirstVisitDate = firstVisitDate ?? DateTime.UtcNow,
            CommunicationPreference = communicationPreference ?? CommunicationType.Email.ToString()
        };

        await AddAsync(groupMember, ct);
        await SaveChangesAsync(ct);
    }

    public async Task AddGroupMembers(int groupId, int[] personIds, int groupRoleId, DateTime? firstVisitDate = null,
        string communicationPreference = "Email", CancellationToken ct = default)
    {
        // Get the personIds that are not already in the group
        var newPersonIds = await Queryable()
            .Where(gm => gm.GroupId == groupId)
            .Select(gm => gm.PersonId)
            .Distinct()
            .ToListAsync(ct);
    
        var personIdsToAdd = personIds.Except(newPersonIds).ToArray();
    
        if (personIdsToAdd.Any())
        {
            var groupMembers = personIdsToAdd.Select(personId => new GroupMember
            {
                PersonId = personId,
                GroupId = groupId,
                GroupRoleId = groupRoleId,
                FirstVisitDate = firstVisitDate ?? DateTime.UtcNow,
                CommunicationPreference = communicationPreference ?? CommunicationType.Email.ToString()
            }).ToList();
    
            await DbContext.AddRangeAsync(groupMembers, ct);
            await SaveChangesAsync(ct);
        }
    }
}

public static class GroupMemberCriteria
{
    // Extra strict requirements as Cell Assistants are also marked leaders
    public static readonly Expression<Func<GroupMember, bool>> IsLeaderOrCanManageFilter = x =>
        x.RecordStatus == RecordStatus.Active &&
        x.GroupRole != null &&
        x.GroupRole.IsLeader &&
        x.GroupRole.CanEdit &&
        x.GroupRole.CanManageMembers;

    public static readonly Expression<Func<GroupMember, bool>> IsLeader = x =>
        x.RecordStatus == RecordStatus.Active &&
        x.GroupRole != null &&
        x.GroupRole.IsLeader;
    
    public static readonly Expression<Func<GroupMember, bool>> IsNotLeaderFilter = x =>
        x.RecordStatus == RecordStatus.Active &&
        x.GroupRole != null &&
        x.GroupRole.IsLeader == false;
}
