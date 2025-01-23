namespace ChurchManager.Domain.Shared;

using System;


public record EventViewModel
{
    public int Id { get; set; }
    public int EventTypeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PhotoUrl { get; set; }
    public int? ChurchId { get; set; }
    public string ChurchName { get; set; }
    public int? ChurchGroupId { get; set; }
    public string ChurchGroupName { get; set; }
    public string ScheduleFriendlyText { get; set; }
    public string ContactPersonPhoneNumber { get; set; }
    public int NumberOfSessions { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; }
    public int Capacity { get; set; }

    // EventType information
    public string EventTypeName { get; set; }
    public EventConfiguration Configuration { get; set; }


}

public record EventConfiguration
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