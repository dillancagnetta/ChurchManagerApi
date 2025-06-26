using System.ComponentModel.DataAnnotations;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances;


/// <summary>
/// Represents a fund where money is designated or restricted for specific purposes within the church.
/// Examples include "Healing School General Support", "Building Maintenance Fund", "Youth Ministry Fund".
/// </summary>
/// <remarks>
/// Funds can support multiple partnerships, and some funds may not relate to partnerships at all.
/// </remarks>
public class Fund: AuditableEntity<int>, IAggregateRoot<int>
{
    /// <summary>
    /// Gets or sets the name of the fund. "Tithes", "Healing School", "Building Project"
    /// </summary>
    [Required, MaxLength(255)] public required string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the unique code identifier for the fund (e.g., "HEAL", "INNER", "LWMEDIA").
    /// </summary>
    [Required, MaxLength(50)] public required string Code { get; set; }
    
    [Required, MaxLength(100)] public required FundType FundType { get; set; } // "GeneralGiving", "Ministry", "Projects"
    
    /// <summary>
    /// Gets or sets the optional description providing additional details about the fund.
    /// </summary>
    [MaxLength(500)] public string? Description { get; set; }

    // Hierarchy
    public int? ParentFundId { get; set; }
    public virtual Fund? ParentFund { get; set; }
    public virtual ICollection<Fund> ChildFunds { get; set; } = new List<Fund>();
}