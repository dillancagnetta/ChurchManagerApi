using System;
using System.Collections.Generic;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using System.Threading.Tasks;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupMemberAttendanceDbRepository : IGenericDbRepository<GroupMemberAttendance>
    {
        Task<(int newConvertsCount, int firstTimersCount, int holySpiritCount)> PeopleStatisticsAsync(IEnumerable<int> groupIds, DateTime startDate = default);
        Task<List<MemberAttendanceReport>> MemberAttendanceReportAsync(int groupId, CancellationToken cts = default);
    }
}
