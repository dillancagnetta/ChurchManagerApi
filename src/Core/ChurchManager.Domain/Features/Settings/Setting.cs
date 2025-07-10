using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Settings;

/// <summary>
///     Represents a setting
/// </summary>
[Table("Setting", Schema = "Common")]
public class Setting : AuditableEntity<int>, IAggregateRoot<int> 
{
    /// <summary>
    ///     Gets or sets the name
    /// </summary>
    public required string Name { get; set; }
    
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
    ///     Gets or sets the Family for which this setting is valid. null for global settings
    /// </summary>
    public int? FamilyId { get; set; }

    /// <summary>
    ///     Gets or sets the metadata settings
    /// </summary>
    public string Metadata { get; set; } = string.Empty;

    #region Navigation properties

    public virtual ChurchGroup? ChurchGroup { get; set; }
    public virtual Church? Church { get; set; }
    public virtual Person? Person { get; set; }
    public virtual Family? Family { get; set; }

    #endregion
}