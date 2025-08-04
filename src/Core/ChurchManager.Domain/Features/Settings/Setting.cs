using System.ComponentModel.DataAnnotations.Schema;
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
    ///     Gets or sets the metadata settings
    /// </summary>
    public string Metadata { get; set; } = string.Empty;
    
}