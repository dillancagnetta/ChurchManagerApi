using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Communications;

public class CommunicationPreferenceType : Entity<int>, IAggregateRoot<int>
{
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    [Required, MaxLength( 100 )]
    public required string Name { get; set; }
    
    [MaxLength( 255 )]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the whether this can be deleted
    /// </summary>
    [DefaultValue(false)] 
    public bool IsSystem { get; set; } = false;
    
    /// <summary>
    /// Gets or sets the default value if a Preference is not explicitly set (created).
    /// </summary>
    [DefaultValue(false)] 
    public bool DefaultNotSetValue { get; set; } = false;
    
    /// <summary>
    /// Gets or sets the default value if a Preference is not explicitly set (created).
    /// </summary>
    [MaxLength(50), DefaultValue("Email")] 
    public CommunicationType DefaultCommunicationType { get; set; } = CommunicationType.Email.Value;
    
    /// <summary>
    /// Gets or sets whether a person can override this preference. this will also determine if we show this setting to the user
    /// </summary>
    [DefaultValue(true)] 
    public bool CanOverride { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<CommunicationPreference> Preferences { get; set; } = new List<CommunicationPreference>();
}

public class CommunicationPreference : Entity<int>, IAggregateRoot<int>
{
    [Required]
    public int PersonId { get; set; }
    
    [Required]
    public int PreferenceTypeId { get; set; }
    
    /// <summary>
    /// Gets or sets the category of the communication.
    /// </summary>
    [MaxLength( 100 )]
    public string? Category { get; set; }
    
    public bool IsEnabled { get; set; } = true;
    
    [MaxLength(50)] public CommunicationType CommunicationType { get; set; } = CommunicationType.Email.Value;
    
    // Navigation properties
    public virtual CommunicationPreferenceType PreferenceType { get; set; }
    public virtual Person Person { get; set; }
}