using System;
using System.Collections.Generic;

namespace ChurchManager.Domain.Shared;

public record GroupAttendanceViewModel
{
    public int Id { get; set; }
    public string ChurchName { get; set; }
    public string GroupName { get; set; }
    public DateTime AttendanceDate { get; set; }
    public bool? DidNotOccur { get; set; }
    public int? AttendanceCount { get; set; }
    public int? FirstTimerCount { get; set; }
    public int? NewConvertCount { get; set; }
    public int? ReceivedHolySpiritCount { get; set; }
    public double AttendanceRate { get; set; }
    public string Notes { get; set; }
    public IEnumerable<string> PhotoUrls { get; set; }
    public MoneyViewModel Offering { get; set; }
    public IEnumerable<GroupMemberAttendanceViewModel> Attendees { get; set; }
}

public record GroupMemberAttendanceViewModel
{
    public DateTime AttendanceDate { get; set; }
    public int GroupMemberId { get; set; }
    public bool? DidAttend { get; set; } = true;
    public bool? IsFirstTime { get; set; }
    public bool? IsNewConvert { get; set; }
    public bool? ReceivedHolySpirit { get; set; }
    public string Note { get; set; }
    public virtual GroupMemberViewModel GroupMember { get; set; }
}

public record MoneyViewModel
{
    public string Currency { get; set; }
    public decimal Amount { get; set; }
}

public record GroupsAverageAttendanceRate
{
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public decimal AverageAttendanceRatePercent { get; set; }
}

public record GroupMemberAttendanceTrackViewModel
{
    public int GroupId { get; set; }
    public int GroupMemberId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public bool DidAttend { get; set; } = true;
    public bool IsFirstTime { get; set; }
    public bool IsNewConvert { get; set; }
    public bool ReceivedHolySpirit { get; set; }
}

public record PeriodComparisonResultsViewModel
{
    public string MetricName { get; set; } 
    public int RecentCount { get; set; }
    public int PreviousCount { get; set; }
    public int AbsoluteChange { get; set; }
    public decimal PercentageChange { get; set; }
    
    public static PeriodComparisonResultsViewModel Create(string metricName, int recentCount,
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

public record AttendanceMetricsComparisonViewModel
{
    public string ReportPeriod { get; set; }
    public PeriodComparisonResultsViewModel NewConvertMetric { get; set; }
    public PeriodComparisonResultsViewModel FirstTimersMetric { get; set; }
    public PeriodComparisonResultsViewModel HolySpiritMetric { get; set; }
}