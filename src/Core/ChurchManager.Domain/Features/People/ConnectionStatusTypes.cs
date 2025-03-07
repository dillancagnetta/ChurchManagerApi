using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.People;

/// <summary>
/// Instead of different person types, track their classification over time
/// Examples: EVENT_REGISTRANT, VISITOR, REGULAR_ATTENDEE, MEMBER
/// Maintains history of someone's journey from event registrant to member
/// Can have multiple classifications (e.g., both EVENT_REGISTRANT and VISITOR)
/// </summary>
public class ConnectionStatusType : Entity<int>
{
    [Required, MaxLength(50)]
    public ConnectionStatus Name { get; set; }
    
    [Required, MaxLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// 0 is the highest priority, and higher numbers are lower priority
    /// </summary>
    public int Priority { get; set; }

    public bool IsSystem { get; set; }
}

[Table("PersonConnectionHistory")]
public class ConnectionStatusHistory: AuditableEntity<int>, IAggregateRoot<int>
{
    public int PersonId { get; set; }
    
    public int ConnectionStatusTypeId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    [MaxLength(500)]
    public string Notes { get; set; }

    #region Navigation

    public virtual Person Person { get; set; }
    
    public virtual ConnectionStatusType ConnectionStatusType { get; set; }

    #endregion
}
