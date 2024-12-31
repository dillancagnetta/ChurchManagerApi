﻿namespace ChurchManager.Domain.Common.Extensions;

public static class PeriodExtensions
{
     public static (DateTime? Start, DateTime? End) ToDateRange(this PeriodType period)
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
                return (null, null);

            default:
                throw new ArgumentOutOfRangeException(nameof(period), period, null);
        }
    }
}