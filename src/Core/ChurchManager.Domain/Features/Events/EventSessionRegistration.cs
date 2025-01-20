using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.Events;

/// <summary>
/// SessionRegistration exists to track which specific sessions a person chose when registering for an event with multiple optional sessions.
/// </summary>
public class EventSessionRegistration : AuditableEntity<int>
{
    public DateTime RegisteredDate { get; set; }
    public int EventRegistrationId { get; set; }
    public int SessionScheduleId { get; set; }
    
    public int PersonId { get; set; }  // Added for query performance
    
    // Navigation Properties
    public virtual EventRegistration EventRegistration { get; set; }
    public virtual EventSessionSchedule SessionSchedule { get; set; }
    
    public virtual Person Person { get; set; }  // Added navigation property

}