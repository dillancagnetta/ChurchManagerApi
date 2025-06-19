using System.ComponentModel.DataAnnotations;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances;

public class Partnership: AuditableEntity<int>, IAggregateRoot<int>
{
    [Required] public required string Name { get; set; }
    [MaxLength(500)] public string? Description { get; set; }
    [Required, MaxLength(50)] public required string Code { get; set; } // e.g., "HEAL", "INNER", "LWMEDIA"
    [MaxLength(100)] public string? Category { get; set; }
    [MaxLength(255)] public string? TrackingCategoryId  { get; private set; } // Xero tracking category ID
    public int? ParentPartnershipId { get; set; }

    #region Navigation

    public virtual Partnership? ParentPartnership { get; set; }

    #endregion
}