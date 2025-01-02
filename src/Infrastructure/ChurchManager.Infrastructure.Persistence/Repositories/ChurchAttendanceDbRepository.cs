using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Churches.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class ChurchAttendanceDbRepository : GenericRepositoryBase<ChurchAttendance>, IChurchAttendanceDbRepository
{
    public ChurchAttendanceDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<ChurchAttendanceAnnualBreakdownVm>> DashboardChurchAttendanceAsync(
        DateTime from, DateTime to, int? churchId = null)
    {
        var query = Queryable().AsNoTracking();

        if (churchId.HasValue && churchId.Value > 0)
        {
            query = query.Where(x => x.ChurchId == churchId.Value);
        }

        var raw = await query
            .Where(x => x.AttendanceDate >= from && x.AttendanceDate <= to)
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
                    Year = x.AttendanceDate.Year,
                    Month = x.AttendanceDate.Month
                },
                (x, e) => new ChurchAttendanceMonthlyTotalsVm
                {
                    Year = x.Year,
                    Month = x.Month,
                    TotalAttendance = e.Sum(y => y.AttendanceCount),
                    TotalNewConverts = e.Sum(y => y.NewConvertCount),
                    TotalFirstTimers = e.Sum(y => y.FirstTimerCount),
                    TotalHolySpirit = e.Sum(y => y.ReceivedHolySpiritCount),
                })
            .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
            .ToListAsync();

        return raw
            .GroupBy(x => x.Year)
            .Select(x => new ChurchAttendanceAnnualBreakdownVm
            {
                Year = x.Key,
                Data = x
            });
    }

    public async Task<dynamic> DashboardChurchAttendanceBreakdownAsync(DateTime from, DateTime to)
    {
        var raw = await Queryable().AsNoTracking()
            .Where(x => x.AttendanceDate >= from && x.AttendanceDate <= to)
            .Select(x => new
            {
                x.AttendanceDate,
                x.AttendanceCount,
                x.MalesCount,
                x.FemalesCount,
                x.ChildrenCount,
            })
            .GroupBy(x => new
                {
                    Year = x.AttendanceDate.Year,
                    Month = x.AttendanceDate.Month
                },
                (x, e) => new
                {
                    Year = x.Year,
                    Month = x.Month,
                    TotalAttendance = e.Sum(y => y.AttendanceCount),
                    TotalMales = e.Sum(y => y.MalesCount),
                    TotalFemales = e.Sum(y => y.FemalesCount),
                    TotalChildren = e.Sum(y => y.ChildrenCount),
                })
            .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
            .ToListAsync();

        return raw
            .GroupBy(x => x.Year)
            .Select(x => new
            {
                Year = x.Key,
                Data = x
            });
    }

    public async Task<AttendanceMetricsComparisonViewModel> AttendanceMetricsComparisonAsync(
        int? churchId,
        ReportPeriodType period, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var periodEnd = now;
        var periodStart = period.GetReportPeriodStartDateFrom(now);
        var previousPeriodStart = period.GetReportPeriodStartDateFrom(periodStart);

        var queryable = Queryable().AsNoTracking();

        if (churchId.HasValue && churchId.Value > 0)
        {
            queryable = queryable.Where(x => x.ChurchId == churchId.Value);
        }

        var result = await queryable
            .Where(ga => ga.AttendanceDate >= previousPeriodStart
                         && ga.AttendanceDate < periodEnd
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

    public async Task<YearlyConversionComparison> YearlyConversionComparisonAsync(int? churchId = null,
        bool includeMonthlyBreakdown = false, CancellationToken ct = default)
    {
        var currentYear = DateTime.UtcNow.Year;
        var startOfPreviousYear = new DateTime(currentYear - 1, 1, 1);

        var queryable = Queryable().AsNoTracking();

        if (churchId is > 0)
        {
            queryable = queryable.Where(x => x.ChurchId == churchId.Value);
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