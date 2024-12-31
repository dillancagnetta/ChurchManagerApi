using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.Extensions;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class GroupAttendanceDbRepository : GenericRepositoryBase<GroupAttendance>, IGroupAttendanceDbRepository
{
    public GroupAttendanceDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PagedResult<GroupAttendanceViewModel>> BrowseGroupAttendance(
        QueryParameter query,
        int groupTypeId,
        int? churchId,
        int? groupId,
        bool withFeedback,
        DateTime? from, DateTime? to,
        CancellationToken ct = default)
    {
        // Paging
        var spec = new BrowseGroupAttendanceSpecification(query, groupTypeId, churchId, groupId, withFeedback, from,
            to);
        var pagedResult = await BrowseAsync<GroupAttendanceViewModel>(query, spec, ct);

        return pagedResult;
    }

    public async Task<dynamic> WeeklyBreakdownForPeriodAsync(int? groupId, ReportPeriod reportPeriod,
        CancellationToken ct)
    {
        var queryable = Queryable()
            .AsNoTracking()
            .Where(x => x.DidNotOccur == null || x.DidNotOccur.Value != true);

        var startDate = reportPeriod.GetStartDate();
        queryable = queryable.Where(x => x.AttendanceDate >= startDate);

        if (groupId.HasValue)
        {
            queryable = queryable.Where(x => x.GroupId == groupId.Value);
        }

        // https://entityframeworkcore.com/knowledge-base/53307101/group-by-week-ef-core-2-1
        return await queryable
            .Select(x => new
            {
                x.AttendanceDate,
                x.AttendanceCount,
                x.NewConvertCount,
                x.FirstTimerCount,
                x.ReceivedHolySpiritCount
            })
            .GroupBy(x => new
                {
                    Week = 1 + (x.AttendanceDate.DayOfYear - 1) / 7
                },
                (x, e) => new
                {
                    x.Week,
                    TotalAttendance = e.Sum(y => y.AttendanceCount),
                    TotalNewConverts = e.Sum(y => y.NewConvertCount),
                    TotalFirstTimers = e.Sum(y => y.FirstTimerCount),
                    TotalHolySpirit = e.Sum(y => y.ReceivedHolySpiritCount),
                })
            .OrderBy(x => x.Week)
            .ToListAsync(ct);
    }

    public async Task<(int newConvertsCount, int firstTimersCount, int holySpiritCount)> PeopleStatisticsAsync(
        IEnumerable<int> groupIds, PeriodType period = PeriodType.ThisYear,
        CancellationToken ct = default)
    {
        var (startDate, endDate) = period.ToDateRange();

        // Optimised query: Group all results together and calculate sum in one go
        var statistics = await Queryable()
            .Where(x =>
                groupIds.Contains(x.GroupId) &&
                x.AttendanceDate >= startDate && x.AttendanceDate <= endDate)
            .GroupBy(x => 1) // Group all results together
            .Select(g => new
            {
                NewConvertsCount = g.Sum(x => x.NewConvertCount ?? 0),
                FirstTimersCount = g.Sum(x => x.FirstTimerCount ?? 0),
                HolySpiritCount = g.Sum(x => x.ReceivedHolySpiritCount ?? 0)
            })
            .FirstOrDefaultAsync(ct);

        return statistics == null
            ? (0, 0, 0)
            : (statistics.NewConvertsCount, statistics.FirstTimersCount, statistics.HolySpiritCount);
    }

    public async Task<GroupAttendanceMetricsComparisonViewModel> GroupAttendanceMetricsComparisonAsync(
        int? GroupId = null, ReportPeriodType period = ReportPeriodType.SixMonths, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var periodEnd = now;
        var periodStart = period.GetReportPeriodStartDateFrom(now);
        var previousPeriodStart = period.GetReportPeriodStartDateFrom(periodStart);

        var queryable = Queryable().AsNoTracking();
        
        if (GroupId.HasValue)
        {
            queryable = queryable.Where(ga => ga.GroupId == GroupId);
        }
        
        var result = await queryable
            .Where(ga => ga.AttendanceDate >= previousPeriodStart
                         && ga.AttendanceDate < periodEnd
                         && (ga.DidNotOccur == null || ga.DidNotOccur == false)
                         && ga.RecordStatus == RecordStatus.Active)
            .GroupBy(ga => ga.AttendanceDate >= periodStart)
            .Select(g => new
            {
                IsRecent = g.Key,
                NewConvertTotal = g.Sum(x => x.NewConvertCount ?? 0),
                FirstTimerTotal = g.Sum(x => x.FirstTimerCount ?? 0),
                ReceivedHolySpiritTotal = g.Sum(x => x.ReceivedHolySpiritCount ?? 0),
            })
            .ToListAsync(ct);

        var recent = result.FirstOrDefault(x => x.IsRecent)?.NewConvertTotal ?? 0;
        var previous = result.FirstOrDefault(x => !x.IsRecent)?.NewConvertTotal ?? 0;
        var newConvertMetric = CreateMetricViewModel("New Converts", recent, previous);
        
        recent = result.FirstOrDefault(x => x.IsRecent)?.FirstTimerTotal ?? 0;
        previous = result.FirstOrDefault(x => !x.IsRecent)?.FirstTimerTotal ?? 0;
        var firstTimersMetric = CreateMetricViewModel("First Timers", recent, previous);
        
        recent = result.FirstOrDefault(x => x.IsRecent)?.ReceivedHolySpiritTotal ?? 0;
        previous = result.FirstOrDefault(x => !x.IsRecent)?.ReceivedHolySpiritTotal ?? 0;
        var holySpiritMetric = CreateMetricViewModel("First Timers", recent, previous);

        return new GroupAttendanceMetricsComparisonViewModel
        {
            ReportPeriod = period.ConvertToString(),
            NewConvertMetric = newConvertMetric,
            FirstTimersMetric = firstTimersMetric,
            HolySpiritMetric = holySpiritMetric,
        };
    }

    private static PeriodComparisonResultsViewModel CreateMetricViewModel(string metricName, int recentCount,
        int previousCount)
    {
        var absoluteChange = recentCount - previousCount;
        var percentageChange = CalculatePercentageChange(previousCount, recentCount);

        return new PeriodComparisonResultsViewModel
        {
            MetricName = metricName,
            RecentCount = recentCount,
            PreviousCount = previousCount,
            AbsoluteChange = absoluteChange,
            PercentageChange = percentageChange
        };
    }

    private static decimal CalculatePercentageChange(int previous, int recent)
    {
        if (previous == 0)
            return recent > 0 ? 100 : 0;

        return Math.Round((decimal)(recent - previous) / previous * 100, 1);
    }
}