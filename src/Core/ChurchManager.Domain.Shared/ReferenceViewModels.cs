namespace ChurchManager.Domain.Shared;

public record GroupReference
{
    public int? GroupTypeId { get; set; }
    public string? GroupTypeName { get; set; }
    
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }
}
    
public record ChurchReference
{
    public int? ChurchGroupId { get; set; }
    public string? ChurchGroupName { get; set; }

    public int? ChurchId { get; set; }
    public string? ChurchName { get; set; }
}