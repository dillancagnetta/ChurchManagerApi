namespace ChurchManager.Domain.Shared;

// -------------------

public record EventTypeViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsSystem { get; set; }
    public string AgeClassification { get; set; }
    public string IconCssClass { get; set; }
    public int? DefaultGroupTypeId { get; set; }
    public string GroupTypeName { get; set; }
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
    public IEnumerable<EventViewModel> Events { get; set; }
}

public record EditEventTypeModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsSystem { get; set; }
    public string IconCssClass { get; set; }
    public string GroupTypeName { get; set; }
}