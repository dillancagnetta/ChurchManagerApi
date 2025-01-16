namespace ChurchManager.Domain.Shared;
public record MemberAttendanceReport
{
    public int GroupId { get; set; }
    public int GroupMemberId { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; }
    public string PhotoUrl { get; set; }
    public bool Meeting1 { get; set; }
    public bool Meeting2 { get; set; }
    public bool Meeting3 { get; set; }
    public bool Meeting4 { get; set; }
    public bool Meeting5 { get; set; }
    public decimal AttendanceRatePercent { get; set; }
}