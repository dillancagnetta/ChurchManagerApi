namespace ChurchManager.Domain.Shared;

public record CommunicationPreferenceViewModel
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public int PreferenceTypeId { get; set; }
    public string? Category { get; set; }
    public required string CommunicationType { get; set; }
    public bool IsEnabled { get; set; }
};

public record CommunicationPreferenceTypeViewModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsSystem { get; set; }
    public bool DefaultNotSetValue { get; set; }
    public bool CanOverride { get; set; }
    public List<CommunicationPreferenceViewModel?> Preferences { get; set; } = new ();
};


/// <summary>
/// Unified view model that represents a preference type with either default or user-overridden values
/// </summary>
public record UnifiedCommunicationPreferenceViewModel
{
    public int PreferenceTypeId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsSystem { get; set; }
    public bool CanOverride { get; set; }
    public string? Category { get; set; }
    public required string CommunicationType { get; set; }
    
    /// <summary>
    /// The effective value - either from user preference or default
    /// </summary>
    public bool IsEnabled { get; set; }
    
    /// <summary>
    /// Indicates whether this value comes from user override (true) or system default (false)
    /// </summary>
    public bool IsUserOverride { get; set; }
    
    /// <summary>
    /// The user's preference ID if this is an override, null if using default
    /// </summary>
    public int? UserPreferenceId { get; set; }
}