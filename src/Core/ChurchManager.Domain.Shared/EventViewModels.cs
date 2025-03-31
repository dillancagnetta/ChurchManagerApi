namespace ChurchManager.Domain.Shared;

using System;


public record EventViewModel
{
    public int Id { get; set; }
    public int EventTypeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PhotoUrl { get; set; }
    public ChurchReference ChurchReference { get; set; }

    public string ContactPersonPhoneNumber { get; set; }
    public int NumberOfSessions { get; set; }
    public string Location { get; set; }
    public string ApprovalStatus { get; set; }
    public int Capacity { get; set; }
    
    public PersonViewModelBasic ContactPerson { get; set; }
    
    public GroupReference ChildCareGroup { get; set; }
    public GroupReference EventRegistrationGroup { get; set; }

    // EventType information
    public string EventTypeName { get; set; }
    public EventConfigurationViewModel Configuration { get; set; }

    private IEnumerable<EventSessionViewModel> _sessions;
    public IEnumerable<EventSessionViewModel> Sessions
    {
        get => _sessions.OrderBy(x => x.SessionOrder);
        set => _sessions = value;
    }

    // Schedule
    public DateTime? StartDate => Sessions.FirstOrDefault()?.StartDate;
    public DateTime? EndDate => Sessions.LastOrDefault()?.EndDate;
    public string StartTime => Sessions.FirstOrDefault()?.StartTime;
    public string EndTime => Sessions.LastOrDefault()?.EndTime;
}

public record EventConfigurationViewModel
{
    public string OnlineSupport { get; set; }
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
    public string Name { get; set; }
    public string Description { get; set; }
    public int SessionOrder { get; set; }
    public int? Capacity { get; set; }

    #region Schedule information

     public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }

    #endregion 
   
    public string Location { get; set; }
    public string OnlineSupport { get; set; }
    public bool IsOnline => !OnlineSupport.Equals("Not Online");
    public string OnlineMeetingUrl { get; set; }
    public bool AttendanceRequired { get; set; }
}

public record EditEventViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
        
    public string Description { get; set; }
        
    public int EventTypeId { get; set; }
        
    public int? ChildCareGroupId { get; set; }
    
    public int? EventRegistrationGroupId { get; set; }
        
    public int ContactPersonId { get; set; }
        
    public string ContactEmail { get; set; }
        
    public string ContactPhone { get; set; }
    public string Location { get; set; }
    public int? Capacity { get; set; }
        
    public int? ChurchGroupId { get; set; }
    public int? ChurchId { get; set; }
    public string PhotoUrl { get; set; }
    
    public List<EventSessionViewModel> Sessions { get; set; } = new();
}
