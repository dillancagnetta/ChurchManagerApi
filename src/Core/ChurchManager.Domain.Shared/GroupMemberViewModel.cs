namespace ChurchManager.Domain.Shared;

public record GroupMemberViewModel
{
    public int GroupId { get; set; }
    public int GroupMemberId { get; set; }
    public int PersonId { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public string? Gender { get; set; }
    public string? PhotoUrl { get; set; }
    public int GroupMemberRoleId { get; set; }
    public string? GroupMemberRole { get; set; }
    public bool IsLeader { get; set; }
    public DateTime? FirstVisitDate { get; set; }
    public string RecordStatus { get; set; } = "Active";

}

public record GroupMemberEditViewModel
{
    public int GroupMemberId { get; set; }
    public AutocompleteResult? Person { get; set; }
    public int GroupRole { get; set; }
    public string? CommunicationPreference { get; set; }
    public string RecordStatus { get; set; } = "Active";
    public DateTime? FirstVisitDate { get; set; }
}

public record PersonGroupsSummaryViewModel
{
    public int GroupId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int? ParentGroupId { get; set; }
    public required string GroupType { get; set; }
    public required string GroupRole { get; set; }
    public string RecordStatus { get; set; } = "Active";
    public bool TakesAttendance { get; set; }
    public bool IsLeader { get; set; }
    public bool CanEdit { get; set; }
    public int MembersCount { get; set; }
    public bool CanManageMembers { get; set; }
    public bool CanView { get; set; }
}