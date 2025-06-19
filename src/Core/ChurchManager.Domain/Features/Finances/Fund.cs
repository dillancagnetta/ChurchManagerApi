using System.ComponentModel.DataAnnotations;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances;

// Where money is designated or restricted for specific purposes
/*
 Examples:
    "Healing School General Support"
    "Building Maintenance Fund"
    "Youth Ministry Fund"
    
    You can have multiple funds supporting the same partnership
    Some funds don't relate to partnerships at all
 */
public class Fund: AuditableEntity<int>, IAggregateRoot<int>
{
    [Required, MaxLength(255)] public required string Name { get; set; }
    [Required, MaxLength(50)] public required string Code { get; set; } // e.g., "HEAL", "INNER", "LWMEDIA"
    [MaxLength(500)] public string? Description { get; set; }
    [Required, MaxLength(50)] public required FundType Type { get; set; } // e.g  "GENERAL", "DESIGNATED", "PARTNERSHIP"

    public DateTime? LastSyncDateTime { get; set; }
    
    public int? PartnershipId { get; set; } //  References any level in the Partnership hierarchy
    
    public virtual Partnership? Partnership { get; set; } // Navigation property to the Partnership entity
}