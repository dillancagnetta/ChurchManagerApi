using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupMemberAttendanceDbRepository : GenericRepositoryBase<GroupMemberAttendance>, IGroupMemberAttendanceDbRepository
    {
        private readonly ChurchManagerDbContext _dbContext;
        private readonly ISqlQueryHandler _sqlQuery;

        public GroupMemberAttendanceDbRepository(ChurchManagerDbContext dbContext, ISqlQueryHandler sqlQuery) : base(dbContext)
        {
            _dbContext = dbContext;
            _sqlQuery = sqlQuery;
        }

        public async Task<(int newConvertsCount, int firstTimersCount, int holySpiritCount)> PeopleStatisticsAsync(IEnumerable<int> groupIds, PeriodType period = PeriodType.ThisYear, CancellationToken ct = default)
        {
            var (startDate, endDate) = period.ToDateRange();

            var attendees = await Queryable()
                .Where(x =>
                    groupIds.Contains(x.GroupId) &&
                    x.AttendanceDate >= startDate && x.AttendanceDate <= endDate)
                .Select(x => new {x.IsNewConvert, x.IsFirstTime, x.ReceivedHolySpirit, x.Id})
                .ToListAsync(ct);

            var newConvertsCount = attendees.Count(x => x.IsNewConvert.GetValueOrDefault());
            var firstTimersCount = attendees.Count(x => x.IsFirstTime.GetValueOrDefault());
            var holySpiritCount = attendees.Count(x => x.ReceivedHolySpirit.GetValueOrDefault());

            return (newConvertsCount, firstTimersCount, holySpiritCount);
        }

        public async Task<List<MemberAttendanceReport>> MemberAttendanceReportAsync(int groupId, CancellationToken ct = default)
        {
            return await _sqlQuery.QueryAsync<MemberAttendanceReport>(
                "SELECT * FROM get_group_attendance_report(@p0)",
                new object[] { groupId }, ct);
        }
        
        public async Task<List<GroupsAverageAttendanceRate>> GroupsAverageAttendanceRateAsync(
            IEnumerable<int> groupIds, 
            PeriodType period = PeriodType.AllTime,
            CancellationToken ct = default)
        {
            var queryable = Queryable()
                .Include(x => x.GroupMember)
                .AsNoTracking();
            
            if (groupIds.Any())
            {
                queryable = queryable.Where(x => groupIds.Contains(x.GroupId));
            }
            
            var (startDate, endDate) = period.ToDateRange();
            queryable = queryable.Where(x => 
                x.AttendanceDate >= startDate &&
                x.AttendanceDate <= endDate);
            
            var averageAttendanceRate = await queryable
                .GroupBy(gma => new { gma.GroupId, gma.Group.Name, gma.AttendanceDate })                .Select(g => new
                {
                    g.Key.GroupId,
                    g.Key.Name,
                    TotalMembers = _dbContext.GroupMember
                        .Count(gm => gm.GroupId == g.Key.GroupId),
                    MembersPresent = g.Count(x => x.DidAttend == true)
                })
                .GroupBy(x => new { x.GroupId, x.Name })
                .Select(g => new GroupsAverageAttendanceRate
                {
                    GroupId = g.Key.GroupId,
                    GroupName = g.Key.Name,
                    AverageAttendanceRatePercent = Math.Round(
                        g.Average(x => x.MembersPresent * 100.0m / (x.TotalMembers == 0 ? 1 : x.TotalMembers)), 
                        1
                    )
                })
                .ToListAsync(ct);
            
            return averageAttendanceRate;
        }
    }
}
