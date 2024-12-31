namespace ChurchManager.Domain.Common.Extensions;

public static class PeriodExtensions
{
    public static (DateTime Start, DateTime End) ToDateRange(this PeriodType period)
    {
        switch (period)
        {
            case PeriodType.Today:
                var todayStart = DateTime.UtcNow.Date;
                var todayEnd = todayStart.AddDays(1).AddTicks(-1);
                return (todayStart, todayEnd);

            case PeriodType.Yesterday:
                var yesterdayStart = DateTime.UtcNow.Date.AddDays(-1);
                var yesterdayEnd = yesterdayStart.AddDays(1).AddTicks(-1);
                return (yesterdayStart, yesterdayEnd);

            case PeriodType.ThisWeek:
                var thisWeekStart = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddTicks(-1);
                return (thisWeekStart, thisWeekEnd);

            case PeriodType.LastWeek:
                var lastWeekStart = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek - 7);
                var lastWeekEnd = lastWeekStart.AddDays(7).AddTicks(-1);
                return (lastWeekStart, lastWeekEnd);

            case PeriodType.ThisMonth:
                var thisMonthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddTicks(-1);
                return (thisMonthStart, thisMonthEnd);

            case PeriodType.LastMonth:
                var lastMonthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(-1);
                var lastMonthEnd = lastMonthStart.AddMonths(1).AddTicks(-1);
                return (lastMonthStart, lastMonthEnd);

            case PeriodType.ThisYear:
                var thisYearStart = new DateTime(DateTime.UtcNow.Year, 1, 1);
                var thisYearEnd = thisYearStart.AddYears(1).AddTicks(-1);
                return (thisYearStart, thisYearEnd);

            case PeriodType.LastYear:
                var start = new DateTime(DateTime.UtcNow.Year, 1, 1).AddYears(-1);
                var end = start.AddYears(1).AddTicks(-1);
                return (start, end);

            case PeriodType.AllTime:
                return (DateTime.MinValue, DateTime.MaxValue);

            default:
                throw new ArgumentOutOfRangeException(nameof(period), period, null);
        }
    }

    public static DateTime GetStartDate(this ReportPeriod period)
    {
        switch (period)
        {
            case ReportPeriod.Week:
                return DateTime.UtcNow.AddDays(-7);

            case ReportPeriod.Month:
                return DateTime.UtcNow.AddMonths(-1);

            case ReportPeriod.Quarter:
                return DateTime.UtcNow.AddMonths(-3);

            case ReportPeriod.SemiAnnual:
                return DateTime.UtcNow.AddMonths(-6);

            case ReportPeriod.Annual:
                return DateTime.UtcNow.AddYears(-1);
            
            default:
                var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
                return sixMonthsAgo;
        }
    }
    
    public static DateTime GetReportPeriodStartDateFrom(this ReportPeriodType period, DateTime start)
    {
        return period switch
        {
            ReportPeriodType.Day => start.AddDays(-1),
            ReportPeriodType.Week => start.AddDays(-7),
            ReportPeriodType.Month => start.AddMonths(-1),
            ReportPeriodType.ThreeMonths => start.AddMonths(-3),
            ReportPeriodType.SixMonths => start.AddMonths(-6),
            ReportPeriodType.OneYear => start.AddYears(-1),
            _ => throw new ArgumentException($"Unsupported period type: {period}")
        };
    }
}