#region

using System.Linq.Expressions;
using AutoMapper;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Extensions;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using ChurchManager.Domain.Shared;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class GroupDbRepository : GenericRepositoryBase<Group>, IGroupDbRepository
{
    private readonly IMapper _mapper;

    public GroupDbRepository(ChurchManagerDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupMemberViewModel>> GroupMembersAsync(int groupId, RecordStatus status,
        CancellationToken ct = default)
    {
        var spec = new GroupMembersSpecification(groupId, status);
        var queryable = ApplySpecification(spec);

        var members = await queryable
            .SelectMany(x => x.Members)
            .Select(x => new GroupMemberViewModel
            {
                PersonId = x.PersonId,
                GroupId = x.GroupId,
                GroupMemberId = x.Id,
                FirstName = x.Person.FullName.FirstName,
                MiddleName = x.Person.FullName.MiddleName,
                LastName = x.Person.FullName.LastName,
                Gender = x.Person.Gender,
                PhotoUrl = x.Person.PhotoUrl,
                GroupMemberRoleId = x.GroupRoleId,
                GroupMemberRole = x.GroupRole.Name,
                IsLeader = x.GroupRole.IsLeader,
                FirstVisitDate = x.FirstVisitDate,
                RecordStatus = x.RecordStatus
            })
            .ToListAsync(ct);

        return members;
    }

    /// <summary>
    /// Gets groups and children that have a specific parent
    /// </summary>
    /*public async Task<IEnumerable<GroupViewModel>> GroupsWithChildrenAsync(int? groupTypeId = null,
        int? parentGroupId = null, int maxDepth = 10, CancellationToken ct = default)
    {
        var query = Queryable()
            .AsNoTracking()
            .Include(x => x.GroupType)
            .Include(x => x.Schedule)
            .Include(x => x.Church)
            .Where(x => x.ParentGroupId == parentGroupId); // null will start at the root of the tree

        if (groupTypeId.HasValue)
        {
            query = query.Where(x => x.GroupTypeId == groupTypeId);
        }

        return await query.Select(GroupProjection(maxDepth)).ToListAsync(ct);
    }*/

    /// <summary>
    /// Gets group and children of that group
    /// </summary>
    public async Task<IEnumerable<GroupViewModel>> GroupWithChildrenAsync(int groupId, int maxDepth = 10,
        CancellationToken ct = default)
    {
        var query = IncludeGroupRelations(Queryable())
                /*.AsNoTracking()
                .Include(x => x.GroupType)
                .Include(x => x.Schedule)
                .Include(x => x.Church)*/
                .Where(x => x.Id == groupId)
            ;

        return await query.Select(GroupProjection(maxDepth)).ToListAsync(ct);
    }

    public async Task<int> GroupMembersCountAsync(int groupId, bool includeLeaders = false,
        CancellationToken ct = default)
    {
        var membersCount = await Queryable()
            .AsNoTracking()
            .Where(x => x.Id == groupId)
            .SelectMany(x => x.Members)
            .Where(m => includeLeaders || !m.GroupRole.IsLeader)
            .CountAsync(ct);

        return membersCount;
    }

    /// <summary>
    /// Group statistics by group type
    /// </summary>
    public async
        Task<(int totalGroupsCount, int activeGroupsCount, int inActiveGroupsCount, int onlineGroupsCount, int
            openedGroupsCount, int closedGroupsCount)> GroupStatisticsAsync(int groupTypeId,
            DateTime? startDate = null,
            CancellationToken ct = default)
    {
        if (startDate == null)
        {
            startDate = DateTime.UtcNow.AddMonths(-6);
        }

        var stats = await Queryable()
            .AsNoTracking()
            .Where(x => x.GroupTypeId == groupTypeId)
            .GroupBy(x => 1) // Group all results together
            .Select(g => new
            {
                TotalCount = g.Count(),
                ActiveCount = g.Count(x => x.RecordStatus == RecordStatus.Active),
                OnlineCount = g.Count(x => x.IsOnline == true),
                OpenedCount = g.Count(x => x.StartDate >= startDate),
                ClosedCount = g.Count(x => x.InactiveDateTime != null &&
                                           x.InactiveDateTime >= startDate &&
                                           x.RecordStatus != RecordStatus.Active)
            })
            .FirstOrDefaultAsync(ct);

        if (stats == null)
        {
            return (0, 0, 0, 0, 0, 0);
        }

        return (
            stats.TotalCount,
            stats.ActiveCount,
            stats.TotalCount - stats.ActiveCount,
            stats.OnlineCount,
            stats.OpenedCount,
            stats.ClosedCount
        );
    }

    /// <summary>
    /// https://michaelceber.medium.com/implementing-a-recursive-projection-query-in-c-and-entity-framework-core-240945122be6
    /// </summary>
    private Expression<Func<Group, GroupViewModel>> GroupProjection(int maxDepth, int currentDepth = 0)
    {
        currentDepth++;

        Expression<Func<Group, GroupViewModel>> result = group => new GroupViewModel
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            Address = group.Address,
            StartDate = group.StartDate,
            ChurchId = group.ChurchId,
            ChurchName = group.Church!.Name,
            ParentGroupId = group.ParentGroupId,
            ParentGroupChurchId = group.ParentGroup!.ChurchId,
            ParentGroupTypeId = group.ParentGroup!.GroupTypeId,
            ParentGroupName = group.ParentGroup!.Name,
            IsOnline = group.IsOnline,
            GroupType = group.GroupType.ToModel(),
            CreatedDate = group.CreatedDate,
            Schedule = group.Schedule.ToModel(),
            Level = currentDepth,
            Groups = currentDepth == maxDepth
                ? new List<GroupViewModel>(0) // Reached maximum depth so stop
                : group.Groups.AsQueryable()
                    .Include(x => x.GroupType)
                    .Select(GroupProjection(maxDepth, currentDepth))
                    .ToList()
        };

        return result;
    }


    /// <summary>
    /// Gets groups and children that have a specific parent
    /// First identifies all groups that are related to the target GroupTypeId (have that type or are connected through the hierarchy)
    /// Loads all of those groups from the database
    ///  Builds the tree structure in memory, respecting the original parentGroupId parameter
    /// avoids the complexity of trying to do recursive filtering in the Expression Tree,
    /// </summary>
    public async Task<IEnumerable<GroupViewModel>> GroupsWithChildrenAsync(
        int? groupTypeId = null, int? churchId = null, int? parentGroupId = null, int maxDepth = 10,
        CancellationToken ct = default)
    {
        // Base query for groups with the specified parent
        var baseQuery = IncludeGroupRelations(Queryable())
            /*.AsNoTracking()
            .Include(x => x.GroupType)
            .Include(x => x.Schedule)
            .Include(x => x.Church)
            .Include(x => x.ParentGroup)*/
            .Where(x => x.ParentGroupId == parentGroupId);

        // Apply churchId filter if specified
        if (churchId.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.ChurchId == churchId);
        }

        // If no groupTypeId filter, just return the tree with churchId filter applied
        if (!groupTypeId.HasValue)
        {
            return await baseQuery.Select(GroupProjection(maxDepth)).ToListAsync(ct);
        }

        // For groupTypeId filter, first find all group IDs that have the target GroupTypeId
        var targetGroupsQuery = Queryable().Where(g => g.GroupTypeId == groupTypeId);
        if (churchId.HasValue)
        {
            targetGroupsQuery = targetGroupsQuery.Where(g => g.ChurchId == churchId);
        }

        var targetGroupIds = await targetGroupsQuery
            .Select(g => g.Id)
            .ToListAsync(ct);

        if (!targetGroupIds.Any())
        {
            // No groups match the filter criteria
            return Enumerable.Empty<GroupViewModel>();
        }

        // Find all related group IDs using a more efficient approach
        var allRelatedGroupIds = new HashSet<int>(targetGroupIds);
        bool added;

        // Find parents (ancestors)
        do
        {
            added = false;
            var parentIds = await Queryable()
                .Where(g => allRelatedGroupIds.Contains(g.Id) && g.ParentGroupId.HasValue)
                .Select(g => g.ParentGroupId.Value)
                .ToListAsync(ct);

            foreach (var id in parentIds)
            {
                if (allRelatedGroupIds.Add(id))
                    added = true;
            }
        } while (added);

        // Find children (descendants)
        do
        {
            added = false;
            var childIds = await Queryable()
                .Where(g => g.ParentGroupId.HasValue && allRelatedGroupIds.Contains(g.ParentGroupId.Value))
                .Select(g => g.Id)
                .ToListAsync(ct);

            foreach (var id in childIds)
            {
                if (allRelatedGroupIds.Add(id))
                    added = true;
            }
        } while (added);

        // Now, get all the relevant groups, but directly select the GroupViewModel
        var allGroupsQuery = IncludeGroupRelations(Queryable())
            /*.AsNoTracking()
            .Include(x => x.GroupType)
            .Include(x => x.Schedule)
            .Include(x => x.Church)
            .Include(x => x.ParentGroup)*/
            .Where(g => allRelatedGroupIds.Contains(g.Id));

        if (churchId.HasValue)
        {
            allGroupsQuery = allGroupsQuery.Where(g => g.ChurchId == churchId);
        }

        // Project directly to GroupViewModel
        var allGroupViewModels = await allGroupsQuery
            .Select(g => g.ToModel())
            .ToListAsync(ct);
        allGroupViewModels.ForEach(g => g.MatchesFilter = g.GroupType!.Id == groupTypeId);

        // Convert to a dictionary for easier lookup
        var groupViewModelDict = allGroupViewModels.ToDictionary(g => g.Id);

        // Filter to get root nodes with the specified parentGroupId
        var rootGroupViewModels = allGroupViewModels
            .Where(g => g!.ParentGroupId == parentGroupId)
            .ToList();

        // Build the tree relationships
        foreach (var groupViewModel in allGroupViewModels)
        {
            // Skip root nodes, as they won't have children in this dictionary
            if (groupViewModel!.ParentGroupId == parentGroupId || !groupViewModel.ParentGroupId.HasValue)
                continue;

            // Find the parent and add this as a child if the parent exists in our dictionary
            if (groupViewModelDict.TryGetValue(groupViewModel.ParentGroupId.Value, out var parentViewModel))
            {
                parentViewModel!.Groups.Add(groupViewModel);
            }
        }

        // Set correct level for each node and determine if nodes have matching descendants
        SetLevelsAndMatchFlags(rootGroupViewModels!, 1, targetGroupIds.ToHashSet());

        // If filtering by GroupTypeId, only include paths that lead to a matching group
        if (groupTypeId.HasValue)
        {
            PruneNonMatchingPaths(rootGroupViewModels!);
        }

        return rootGroupViewModels;
    }

    // Helper method to set the correct level for each node and flag nodes in paths to matches
    private void SetLevelsAndMatchFlags(List<GroupViewModel> groups, int level, HashSet<int> targetGroupIds)
    {
        foreach (var group in groups)
        {
            group.Level = level;

            if (group.Groups.Any())
            {
                SetLevelsAndMatchFlags(group.Groups.ToList(), level + 1, targetGroupIds);

                // If any child has a matching descendant, this node is in a path to a match
                if (group.Groups.Any(g => g.MatchesFilter || g.HasMatchingDescendant))
                {
                    group.HasMatchingDescendant = true;
                }
            }
        }
    }

    // Helper method to remove branches that don't lead to matching groups
    private void PruneNonMatchingPaths(List<GroupViewModel> groups)
    {
        for (int i = groups.Count - 1; i >= 0; i--)
        {
            var group = groups[i];

            if (group.Groups.Any())
            {
                PruneNonMatchingPaths(group.Groups.ToList());
            }

            // Keep this node only if it matches the filter or has a descendant that does
            if (!group.MatchesFilter && !group.HasMatchingDescendant)
            {
                groups.RemoveAt(i);
            }
        }
    }

    private GroupViewModel BuildGroupViewModel(Group group, Dictionary<int, Group> allGroups, int maxDepth,
        int currentDepth = 0)
    {
        currentDepth++;

        var viewModel = new GroupViewModel
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            Address = group.Address,
            StartDate = group.StartDate,
            ChurchId = group.ChurchId,
            ChurchName = group.Church?.Name,
            ParentGroupId = group.ParentGroupId,
            ParentGroupChurchId = group.ParentGroup?.ChurchId,
            ParentGroupTypeId = group.ParentGroup?.GroupTypeId,
            ParentGroupName = group.ParentGroup?.Name,
            IsOnline = group.IsOnline,
            GroupType = _mapper.Map<GroupTypeViewModel>(group.GroupType),
            CreatedDate = group.CreatedDate,
            Schedule = group.Schedule != null ? _mapper.Map<ScheduleViewModel>(group.Schedule) : null,
            Level = currentDepth,
            Groups = new List<GroupViewModel>()
        };

        // Add children if we haven't reached max depth
        if (currentDepth < maxDepth)
        {
            var childGroups = allGroups.Values
                .Where(g => g.ParentGroupId == group.Id)
                .ToList();

            foreach (var childGroup in childGroups)
            {
                viewModel.Groups.Add(BuildGroupViewModel(childGroup, allGroups, maxDepth, currentDepth));
            }
        }

        return viewModel;
    }
    
    private IQueryable<Group> IncludeGroupRelations(IQueryable<Group> query)
    {
        return query
            .AsNoTracking()
            .Include(x => x.GroupType)
            .Include(x => x.Schedule)
            .Include(x => x.Church)
            .Include(x => x.ParentGroup);
    }
}