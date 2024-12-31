using System;
using System.Collections.Generic;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupMemberAttendanceDbRepository : IGenericDbRepository<GroupMemberAttendance>
    {
        Task<(int newConvertsCount, int firstTimersCount, int holySpiritCount)> PeopleStatisticsAsync(IEnumerable<int> groupIds, PeriodType period = PeriodType.ThisYear, CancellationToken ct = default);
        Task<List<MemberAttendanceReport>> MemberAttendanceReportAsync(int groupId, CancellationToken cts = default);
        Task<List<GroupsAverageAttendanceRate>> GroupsAverageAttendanceRateAsync(IEnumerable<int> groupIds, PeriodType period = PeriodType.AllTime, CancellationToken ct = default);
    }
}
