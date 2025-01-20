using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Events;

public class EventSession : AuditableEntity<int>
{
    [MaxLength( 100 )]
    public string Name { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }
    
    /// <summary>
    /// indicates whether attendance at this particular session is mandatory for anyone registering for the event
    /// </summary>
    public bool AttendanceRequired { get; set; }
    
    public int SessionOrder { get; set; }
    
    /// <summary>
    /// The event (id) that this session belongs to.
    /// </summary>
    public int EventId { get; set; }
    
    # region Navigation
    
    /// <summary>
    /// The event (id) that this session belongs to.
    /// </summary>
    public virtual Event Event { get; set; }
    
    public virtual ICollection<EventSessionSchedule> SessionSchedules { get; set; }
    public virtual ICollection<EventSessionRegistration> SessionRegistrations { get; set; }
    
    # endregion
}

public class EventSessionRegistration : AuditableEntity<int>
{
    public DateTime RegisteredDate { get; set; }

    public int EventRegistrationId { get; set; }
    public int SessionScheduleId { get; set; }
    
    // Navigation Properties
    public virtual EventRegistration EventRegistration { get; set; }
    public virtual EventSessionSchedule EventSessionSchedule { get; set; }
}

public class EventSessionSchedule : AuditableEntity<int>
{
    public int EventSessionId { get; set; }

    [Required]
    public DateTime StartDateTime { get; set; }  // Combined date and time
    
    [Required]
    public DateTime EndDateTime { get; set; }    // Combined date and time
    
    [MaxLength(200)]
    public string Location { get; set; }
    
    [MaxLength(500)]
    public string Notes { get; set; }
    
    public bool IsOnline { get; set; }
   
    [MaxLength(200)]
    public string OnlineMeetingUrl { get; set; }
    
    public bool IsCancelled { get; set; }
    
    public string CancellationReason { get; set; }

    // Navigation Properties
    public virtual EventSession EventSession { get; set; }
    public virtual ICollection<EventSessionRegistration> SessionRegistrations { get; set; }
}
