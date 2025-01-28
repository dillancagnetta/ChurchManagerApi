using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupMemberDbRepository : IGenericDbRepository<GroupMember>
    {
        IQueryable<GroupMember> GetByGroupId(int groupId);
        IQueryable<GroupMember> GetLeaders(int groupId);
        Task<(int peopleCount, int leadersCount)> PeopleAndLeadersInGroupsAsync(int groupTypeId, CancellationToken ct = default);
        Task<(int peopleCount, int leadersCount)> PeopleAndLeadersInGroupAsync(int groupId, CancellationToken ct = default);
        Task AddGroupMember(int groupId, int personId, int groupRoleId, DateTime? firstVisitDate = null, string communicationPreference = "Email", CancellationToken ct = default);
        Task AddGroupMembers(int groupId, int[] personId, int groupRoleId, DateTime? firstVisitDate = null, string communicationPreference = "Email", CancellationToken ct = default);
    }
}
