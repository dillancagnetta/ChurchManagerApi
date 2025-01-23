using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Events;

/*
Given these requirements:

    - Event registrants automatically become group members (temporary)
    - Need to track attendance at events via existing group attendance system
    - Need to track the journey from event registrant to church member
    - Must maintain event registration history
    - Need clean, performant queries
    - Already have groups/attendance infrastructure
 */

/// <summary>
/// Event is the logical level for church/group association
/// Sessions are more about timing and content
/// Keeps session structure focused on scheduling
/// </summary>
public class Event : AuditableEntity<int>, IAggregateRoot<int>
{
    [MaxLength( 100 ), Required]
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string PhotoUrl { get; set; }
    
    public int EventTypeId { get; set; }

    /// <summary>
    /// Gets or sets the Id of the <see cref="Church"/> that contains this event belongs to.
    /// </summary>
    public int? ChurchId { get; set; }
    
    /// <summary>
    /// Gets or sets the Id of the <see cref="ChurchGroup"/> that contains this event belongs to.
    /// </summary>
    public int? ChurchGroupId { get; set; }

    /// <summary>
    /// Gets or sets the Id of the <see cref="ChurchGroup"/> that contains this event belongs to.
    /// </summary>
    public int ScheduleId { get; set; }
    
    /// <summary>
    /// Gets or sets the  child care group for the event
    /// </summary>
    public int? ChildCareGroupId { get; set; }
    
    /// <summary>
    /// Group for Event Registrants
    /// </summary>
    public int? EventRegistrationGroupId { get; set; }
    
    /// <summary>
    /// Gets or sets the Id of the <see cref="Person"/> for the Event's contact person. This property is required.
    /// </summary>
    public int ContactPersonId { get; set; }
    
    [MaxLength( 75 )]
    public string ContactEmail { get; set; }
    
    [MaxLength( 50 )]
    public string ContactPhone { get; set; }
    
    public Review Review { get; set; }
    
    public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.PendingApproval;
    
    /// <summary>
    /// Gets or sets the  events attendance capacity
    /// </summary>
    public int? Capacity { get; set; }
    
    [MaxLength(200)]
    public string Location { get; set; }
    
    #region Navigation
    public virtual Church Church { get; set; }
    public virtual ChurchGroup ChurchGroup { get; set; }
    public virtual Schedule Schedule { get; set; }
    public virtual Person ContactPerson { get; set; }
    public virtual Group ChildCareGroup { get; set; }
    public virtual Group EventRegistrationGroup { get; set; }
    
    public virtual EventType EventType { get; set; }
    
    public virtual ICollection<EventSession> Sessions { get; set; }
    
    public virtual ICollection<EventRegistration> Registrations { get; set; }
    
    #endregion
}
