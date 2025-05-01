namespace ChurchManager.Domain.Shared;

public record GroupViewModel
{
    public int Id { get; set; }
    public GroupTypeViewModel? GroupType { get; set; }
    public int? ChurchId { get; set; }
    public int? ParentGroupId { get; set; }
    public int? ParentGroupTypeId { get; set; }
    public int? ParentGroupChurchId { get; set; }
    public string? ParentGroupName { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public bool? IsOnline { get; set; }

    public DateTime CreatedDate { get; set; }

    public IEnumerable<GroupViewModel> Groups { get; set; } = [];
    public int Level { get; set; } // Tree Depth level
    public ScheduleViewModel? Schedule { get; set; }
}

public record GroupTypeViewModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? GroupTerm { get; set; } 
    public string? GroupMemberTerm { get; set; }
    public bool TakesAttendance { get; set; }
    public bool IsSystem { get; set; }
    public string? IconCssClass { get; set; }
}

public record ScheduleViewModel
{
    public string? ScheduleText { get; set; } // Schedule Friendly Text
    public string? iCalendarContent { get; set; } // Schedule Calendar Content
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? MeetingTime { get; set; } // Start Meeting Time or could be start time of event
    public string? EndMeetingTime { get; set; } // End Meeting Time or end time of event i.e. the final closing time
    public string? RecurrenceRule { get; set; }
    public string? Frequency { get; set; }
    public string Timezone { get; set; } = "South Africa Standard Time";
}