using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.Events;

public class EventRegistration : AuditableEntity<int>
{
    [Required]
    public DateTime RegistrationDate { get; set; }
    
    // Child care registration 
    public bool RequiresChildCare { get; set; }
    
    public int? NumberOfChildren { get; set; }
    
    /// <summary>
    /// The person registering for the event
    /// </summary>
    public int PersonId { get; set; }
    
    public int? RegisteredByPersonId { get; set; }
    
    /// <summary>
    /// The registration group the person is added to
    /// </summary>
    public int GroupId { get; set; }  

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