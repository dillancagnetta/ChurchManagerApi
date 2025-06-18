using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Events;

/// Event: "3-Day Conference"
/// Sessions:
///    - Day 1 Morning: "Keynote"
///    - Day 1 Afternoon: "Workshop A"
///    - Day 2 Morning: "Workshop B"
///    - Day 2 Afternoon: "Workshop C"
///    - Day 3: "Closing"
public class EventSession : AuditableEntity<int>
{
    [MaxLength( 100 )]
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public int? Capacity { get; set; }
    
    /// <summary>
    /// indicates whether attendance at this particular session is mandatory for anyone registering for the event
    /// </summary>
    public bool AttendanceRequired { get; set; } = false;
    
    public int SessionOrder { get; set; }
    
    /// <summary>
    /// The event (id) that this session belongs to.
    /// </summary>
    public int EventId { get; set; }
    
    // Schedule information
    /// <summary>
    /// Gets or sets the ScheduleId of the <see cref="Schedule"/> that contains this event belongs to.
    /// </summary>
    public int ScheduleId { get; set; }
    
    [MaxLength(200)]
    public string? Location { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }

    public OnlineSupport OnlineSupport { get; set; } = OnlineSupport.Unknown.Value;
   
    [MaxLength(200)]
    public string? OnlineMeetingUrl { get; set; }
    
    public bool IsCancelled { get; set; }
    
    public string? CancellationReason { get; set; }
    
    # region Navigation
    
    /// <summary>
    /// The event (id) that this session belongs to.
    /// </summary>
    public virtual Event? Event { get; set; }
    
    public virtual Schedule? Schedule { get; set; }
    
    public virtual ICollection<EventSessionRegistration> SessionRegistrations { get; set; } = Enumerable.Empty<EventSessionRegistration>().ToList();
    
    # endregion
    
    #region Methods
    public (DateOnly? StartDate, TimeOnly? StartTime) SessionStartDateTime()
    {
        return (Schedule?.StartDate, Schedule?.StartTime);
    }
    
    public (DateOnly? EndDate, TimeOnly? EndTime) SessionEndDateTime()
    {
        return (Schedule?.EndDate, Schedule?.EndTime);
    }
    #endregion
    
}