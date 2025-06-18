namespace ChurchManager.Domain.Shared;

public record ChurchAttendanceViewModel
{
    public int Id { get; set; }
    public required string ChurchName { get; set; }
    public required string ChurchGroupName { get; set; }
    public required string AttendanceTypeName { get; set; }
    public DateTime AttendanceDate { get; set; }
    public bool? DidNotOccur { get; set; }
    public int? AttendanceCount { get; set; }
    public int? FirstTimerCount { get; set; }
    public int? NewConvertCount { get; set; }
    public int? ReceivedHolySpiritCount { get; set; }
    public double AttendanceRate { get; set; }
    public string? Notes { get; set; }
    public IEnumerable<string>? PhotoUrls { get; set; }

    public int? MalesCount { get; set; }

    public int? FemalesCount { get; set; }

    public int? ChildrenCount { get; set; }

    public int? TeensCount { get; set; }
    //public MoneyViewModel Offering { get; set; }
}
