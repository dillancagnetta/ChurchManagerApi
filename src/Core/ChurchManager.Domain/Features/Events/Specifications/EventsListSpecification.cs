using Ardalis.Specification;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Specifications;

namespace ChurchManager.Domain.Features.Events.Specifications;

public class EventsListSpecification: PermissionSpecification<Event, EventViewModel>
{
    public EventsListSpecification(IEnumerable<int> allowedEventIds = null, 
        int? eventTypeId = null, 
        int? churchGroupId = null, 
        int? churchId = null, 
        DateTime? from = null, 
        DateTime? to = null, 
        bool? isOnline = null,
        bool? includeDetails = null): base(allowedEventIds)
    {
        Query.AsNoTracking();
        
        if (eventTypeId.HasValue)   
        {
            Query.Where(x => x.EventTypeId == eventTypeId);
        }
        
        if (churchGroupId is > 0)
        {
            Query.Include(x => x.ChurchGroup);
            Query.Where(x => x.ChurchGroupId == churchGroupId);
        }
        
        if (churchId is > 0)
        {
            Query.Include(x => x.Church);
            Query.Where(x => x.ChurchId == churchId);
        }
        
        if (isOnline.HasValue)
        {
            Query.Include(x => x.EventType);
            Query.Where(x => x.EventType.OnlineSupport != OnlineSupport.NotOnline.Value);
        }
        
        if (from.HasValue)
        {
            Query.Include(x => x.Schedule);
            Query.Where(x => x.Schedule.StartDate >= from);
        }
        
        if (to.HasValue)
        {
            Query.Include(x => x.Schedule);
            Query.Where(x => x.Schedule.EndDate <= from);
        }

        if (includeDetails.HasValue)
        {
            Query.Include(x => x.EventType);
            Query.Include(x => x.Church);
            Query.Include(x => x.ChurchGroup);
            Query.Include(x => x.Schedule);
            Query.Include(x => x.Sessions);
        }

        Query.Select(ExpressionExtensions.SelectEventWithDetails);
    }
}