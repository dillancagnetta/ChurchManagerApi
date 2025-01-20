using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.Events;

[Table("EventType")]

public class EventType: AuditableEntity<int>
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; }
    
    [MaxLength(100)]
    public string Description { get; set; }
    
    public bool RequiresRegistration { get; set; }
    public bool AllowFamilyRegistration { get; set; }
    public bool TakeAttendance { get; set; }
    public bool RequiresChildInfo { get; set; }
    public bool SupportsOnline { get; set; } 
    
    public bool IsSystem { get; set; } = false;

    public string IconCssClass { get; set; } = "heroicons_solid:academic-cap";

    public ChildCare ChildCare { get; set; }

    public AgeClassification? AgeClassification { get; set; }
    
    public int? DefaultGroupTypeId { get; set; }

    #region Navigation

    public virtual GroupType DefaultGroupType { get; set; }

    #endregion
}

[Owned]
public record ChildCare
{
    public bool HasChildCare { get; set; } = false;

    public int? MinChildAge { get; set; }
    
    public int? MaxChildAge { get; set; }
}