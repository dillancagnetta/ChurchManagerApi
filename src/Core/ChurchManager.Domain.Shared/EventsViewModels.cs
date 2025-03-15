namespace ChurchManager.Domain.Shared;

// -------------------

public record EventTypeViewModel : EventConfigurationViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsSystem { get; set; }
    public string IconCssClass { get; set; }
    public string GroupTypeName { get; set; }
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