using System.ComponentModel.DataAnnotations;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.Events;

public class EventSessionSchedule : AuditableEntity<int>
{
    public int EventSessionId { get; set; }

    public DateTime? StartDateTime { get; set; }  // Combined date and time
    
    public DateTime? EndDateTime { get; set; }    // Combined date and time
    
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