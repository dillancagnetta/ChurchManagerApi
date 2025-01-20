using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.Events;

/// <summary>
/// EventRegistration tells us:
///     Person registered for the event
///     Which group they're in
///     Overall registration status 
/// </summary>
///
/// Event: "3-Day Conference"
/// Sessions:
///    - Day 1 Morning: "Keynote"
///    - Day 1 Afternoon: "Workshop A"
///    - Day 2 Morning: "Workshop B"
///    - Day 2 Afternoon: "Workshop C"
///    - Day 3: "Closing"
///
/// When someone registers:
/// 1. EventRegistration: "John is registered for the conference"
/// 2. SessionRegistration: "John selected Day 1 Morning, Day 2 Afternoon, Day 3"
public class EventRegistration : AuditableEntity<int>
{
    [Required]
    public DateTime RegistrationDate { get; set; }
    
    public bool RegisteredForAllSessions { get; set; }
    
    // Child care registration 
    public bool RequiresChildCare { get; set; }
   
    public int? NumberOfChildren { get; set; }
    
    /// <summary>
    /// The person registering for the event
    /// </summary>
    public int PersonId { get; set; }
    
    public int? RegisteredByPersonId { get; set; }
    
    /// <summary>
    /// The registration group the person is added to (optional)
    /// </summary>
    public int? GroupId { get; set; }  

    /// <summary>
    /// The event (id) that this session belongs to.
    /// </summary>
    public int EventId { get; set; }

    public RegistrationStatus Status { get; set; }

    # region Navigation
    
    /// <summary>
    /// The event (id) that this session belongs to.
    /// </summary>
    public virtual Event Event { get; set; }
    
    public virtual Person Person { get; set; }
    public virtual Group Group { get; set; }
    public virtual Person RegisteredByPerson { get; set; }
    
    public virtual ICollection<EventSessionRegistration> SessionRegistrations { get; set; }
    
    # endregion
}