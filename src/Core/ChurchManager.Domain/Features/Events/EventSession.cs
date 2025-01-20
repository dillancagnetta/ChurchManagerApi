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
    public string Name { get; set; }
    
    public string Description { get; set; }
    
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
    
    # region Navigation
    
    /// <summary>
    /// The event (id) that this session belongs to.
    /// </summary>
    public virtual Event Event { get; set; }
    
    public virtual ICollection<EventSessionSchedule> SessionSchedules { get; set; }
    public virtual ICollection<EventSessionRegistration> SessionRegistrations { get; set; }
    
    # endregion
}