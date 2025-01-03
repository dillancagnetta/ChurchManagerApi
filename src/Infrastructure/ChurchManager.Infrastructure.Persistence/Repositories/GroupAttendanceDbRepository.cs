using System.Globalization;
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

    public async Task<AttendanceMetricsComparisonViewModel> GroupAttendanceMetricsComparisonAsync(
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
        var newConvertMetric = PeriodComparisonResultsViewModel.Create("New Converts", recent, previous);
        
        recent = result.FirstOrDefault(x => x.IsRecent)?.FirstTimerTotal ?? 0;
        previous = result.FirstOrDefault(x => !x.IsRecent)?.FirstTimerTotal ?? 0;
        var firstTimersMetric = PeriodComparisonResultsViewModel.Create("First Timers", recent, previous);
        
        recent = result.FirstOrDefault(x => x.IsRecent)?.ReceivedHolySpiritTotal ?? 0;
        previous = result.FirstOrDefault(x => !x.IsRecent)?.ReceivedHolySpiritTotal ?? 0;
        var holySpiritMetric = PeriodComparisonResultsViewModel.Create("Holy Spirit", recent, previous);

        return new AttendanceMetricsComparisonViewModel
        {
            ReportPeriod = period.ConvertToString(),
            NewConvertMetric = newConvertMetric,
            FirstTimersMetric = firstTimersMetric,
            HolySpiritMetric = holySpiritMetric,
        };
    }

    public async Task<YearlyConversionComparison> YearlyConversionComparisonAsync(
        int groupTypeId, 
        int? churchId = null, 
        int? groupId = null,
        bool includeMonthlyBreakdown = false, 
        CancellationToken ct = default)
    {
        var currentYear = DateTime.UtcNow.Year;
        var startOfPreviousYear = new DateTime(currentYear - 1, 1, 1);

        var queryable = Queryable()
            .Include(x => x.Group)
            .AsNoTracking();
        
        queryable = queryable.Where(x => x.Group.GroupTypeId == groupTypeId);
        
        if (groupId.HasValue)
        {
            queryable = queryable.Where(ga => ga.GroupId == groupId);
        }
        
        if (churchId.HasValue)
        {
            queryable = queryable.Where(ga => ga.Group.ChurchId == churchId);
        }
        
        if (includeMonthlyBreakdown)
        {
            // Group by year and month when breakdown is needed
            var detailedResult = await queryable
                .Where(ga => ga.AttendanceDate >= startOfPreviousYear
                             && ga.RecordStatus == RecordStatus.Active)
                .GroupBy(ga => new { Year = ga.AttendanceDate.Year, Month = ga.AttendanceDate.Month })
                .Select(g => new MonthlyData(
                    g.Key.Year,
                    g.Key.Month,
                    g.Sum(x => x.FirstTimerCount ?? 0),
                    g.Sum(x => x.NewConvertCount ?? 0)
                ))
                .ToListAsync(ct);

            var currentYearMetrics = CalculateYearlyMetricsWithBreakdown(
                detailedResult.Where(x => x.Year == currentYear).ToList(),
                currentYear
            );

            var previousYearMetrics = CalculateYearlyMetricsWithBreakdown(
                detailedResult.Where(x => x.Year == currentYear - 1).ToList(),
                currentYear - 1
            );

            return new YearlyConversionComparison
            {
                CurrentYear = currentYearMetrics,
                PreviousYear = previousYearMetrics,
                ConversionRateChange =
                    currentYearMetrics.ConversionPercentage - previousYearMetrics.ConversionPercentage
            };
        }
        else
        {
            // Simple yearly totals when breakdown is not needed
            var yearlyResult = await queryable
                .Where(ga => ga.AttendanceDate >= startOfPreviousYear
                             && ga.RecordStatus == RecordStatus.Active)
                .GroupBy(ga => ga.AttendanceDate.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    FirstTimers = g.Sum(x => x.FirstTimerCount ?? 0),
                    NewConverts = g.Sum(x => x.NewConvertCount ?? 0)
                })
                .ToListAsync(ct);

            var currentYearMetrics = CalculateYearlyMetrics(
                yearlyResult.FirstOrDefault(x => x.Year == currentYear),
                currentYear
            );

            var previousYearMetrics = CalculateYearlyMetrics(
                yearlyResult.FirstOrDefault(x => x.Year == currentYear - 1),
                currentYear - 1
            );

            return new YearlyConversionComparison
            {
                CurrentYear = currentYearMetrics,
                PreviousYear = previousYearMetrics,
                ConversionRateChange =
                    currentYearMetrics.ConversionPercentage - previousYearMetrics.ConversionPercentage
            };
        }
    }

    private YearlyConversionMetrics CalculateYearlyMetrics(dynamic yearData, int year)
    {
        var firstTimers = yearData?.FirstTimers ?? 0;
        var newConverts = yearData?.NewConverts ?? 0;

        return new YearlyConversionMetrics
        {
            Year = year,
            FirstTimers = firstTimers,
            NewConverts = newConverts,
            ConversionPercentage = firstTimers == 0
                ? 0
                : Math.Round((decimal)newConverts / firstTimers * 100, 1)
        };
    }

    private YearlyConversionMetrics CalculateYearlyMetricsWithBreakdown(List<MonthlyData> yearData, int year)
    {
        var totalFirstTimers = yearData.Sum(x => x.FirstTimers);
        var totalNewConverts = yearData.Sum(x => x.NewConverts);

        var monthlyBreakdown = Enumerable.Range(1, 12)
            .Select(month =>
            {
                var monthData = yearData.FirstOrDefault(x => x.Month == month);
                var firstTimers = monthData?.FirstTimers ?? 0;
                var newConverts = monthData?.NewConverts ?? 0;

                return new MonthlyConversionMetrics
                {
                    Month = month,
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    FirstTimers = firstTimers,
                    NewConverts = newConverts,
                    ConversionPercentage = firstTimers == 0
                        ? 0
                        : Math.Round((decimal)newConverts / firstTimers * 100, 1)
                };
            })
            .ToList();

        return new YearlyConversionMetrics
        {
            Year = year,
            FirstTimers = totalFirstTimers,
            NewConverts = totalNewConverts,
            ConversionPercentage = totalFirstTimers == 0
                ? 0
                : Math.Round((decimal)totalNewConverts / totalFirstTimers * 100, 1),
            MonthlyBreakdown = monthlyBreakdown
        };
    }
    
    private record MonthlyData(int Year, int Month, int FirstTimers, int NewConverts);
}