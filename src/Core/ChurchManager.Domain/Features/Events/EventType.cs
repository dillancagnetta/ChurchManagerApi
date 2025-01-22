using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;
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
    public bool AllowNonFamilyRegistration { get; set; }
    public bool TakesAttendance { get; set; }
    public bool RequiresChildInfo { get; set; }
    
    public OnlineSupport OnlineSupport { get; set; } 
    
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

public class OnlineSupport : Enumeration<OnlineSupport, string>
{
    public OnlineSupport() { Value = "Unknown"; }
        
    public OnlineSupport(string value) => Value = value;

    public static OnlineSupport OnlineOnly = new("Online Only");
    public static OnlineSupport Both = new("Both");
    public static OnlineSupport NotOnline = new("Not Online");
    public static OnlineSupport Unknown = new("Unknown");
    // Implicit conversion from string
    public static implicit operator OnlineSupport(string value) => new(value);
}
