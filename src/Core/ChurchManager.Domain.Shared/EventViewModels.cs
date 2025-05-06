namespace ChurchManager.Domain.Shared;

using System;


public record EventViewModel
{
    public int Id { get; set; }
    public int EventTypeId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? PhotoUrl { get; set; }
    public ChurchReference? ChurchReference { get; set; }

    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public int NumberOfSessions { get; set; }
    public string? Location { get; set; }
    public string? ApprovalStatus { get; set; }
    public int Capacity { get; set; }
    
    public PersonViewModelBasic? ContactPerson { get; set; }
    
    public GroupReference? ChildCareGroup { get; set; }
    public GroupReference? EventRegistrationGroup { get; set; }

    // EventType information
    public string? EventTypeName { get; set; }
    public EventConfigurationViewModel? Configuration { get; set; }

    private IEnumerable<EventSessionViewModel> _sessions = [];
    public IEnumerable<EventSessionViewModel> Sessions
    {
        get => _sessions.OrderBy(x => x.SessionOrder);
        set => _sessions = value;
    }

    // Schedule
    public DateOnly? StartDate => Sessions.FirstOrDefault()?.StartDate;
    public DateOnly? EndDate => Sessions.LastOrDefault()?.EndDate;
    public TimeOnly? StartTime => Sessions.FirstOrDefault()?.StartTime;
    public TimeOnly? EndTime => Sessions.LastOrDefault()?.EndTime;
    
    // Registration
    public DateTime? RegistrationStartDate  { get; set; }
    public DateTime? RegistrationEndDate  { get; set; }
}

public record EventConfigurationViewModel
{
    public string OnlineSupport { get; set; } = "Both";
    public bool RequiresRegistration { get; set; }
    public bool AllowFamilyRegistration { get; set; }
    public bool AllowNonFamilyRegistration { get; set; }
    public bool RequiresChildInfo { get; set; }
    public bool TakesAttendance { get; set; }
    // ChildCare information
    public bool? HasChildCare { get; set; }
    public int? MinChildAge { get; set; }
    public int? MaxChildAge { get; set; }
    public bool IsOnline => !OnlineSupport.Equals("Not Online");
}

public record EventSessionViewModel
{
    public int? Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int SessionOrder { get; set; }
    public int? Capacity { get; set; }

    #region Schedule information

     public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }

    #endregion 
   
    public string? Location { get; set; }
    public string OnlineSupport { get; set; } = "Both";
    public bool IsOnline => !OnlineSupport.Equals("Not Online");
    public string? OnlineMeetingUrl { get; set; }
    public bool AttendanceRequired { get; set; }
}

public record EditEventViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
        
    public string? Description { get; set; }
        
    public int EventTypeId { get; set; }
        
    public int? ChildCareGroupId { get; set; }
    
    public int? EventRegistrationGroupId { get; set; }
        
    public int ContactPersonId { get; set; }
        
    public string? ContactEmail { get; set; }
        
    public string? ContactPhone { get; set; }
    public string? Location { get; set; }
    public int? Capacity { get; set; }
        
    public int? ChurchGroupId { get; set; }
    public int? ChurchId { get; set; }
    public string? PhotoUrl { get; set; }
    
    public List<EventSessionViewModel> Sessions { get; set; } = new();
}
