using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Settings;

/// <summary>
///     Represents a setting
/// </summary>
public class Setting : AuditableEntity<int>, IAggregateRoot<int> 
{
    /// <summary>
    ///     Gets or sets the name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    ///     Gets or sets the church group for which this setting is valid. null for global settings
    /// </summary>
    public int? ChurchGroupId { get; set; }

    /// <summary>
    ///     Gets or sets the church for which this setting is valid. null for global settings
    /// </summary>
    public int? ChurchId { get; set; }
    
    /// <summary>
    ///     Gets or sets the person for which this setting is valid. null for global settings
    /// </summary>
    public int? PersonId { get; set; }

    /// <summary>
    ///     Gets or sets the metadata settings
    /// </summary>
    public string Metadata { get; set; }

    #region Navigation properties

    public ChurchGroup ChurchGroup { get; set; }
    public Church Church { get; set; }
    public Person Person { get; set; }

    #endregion
}