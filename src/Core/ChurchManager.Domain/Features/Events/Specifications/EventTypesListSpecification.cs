using Ardalis.Specification;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Specifications;

namespace ChurchManager.Domain.Features.Events.Specifications;

public class EventTypesListSpecification : PermissionSpecification<EventType, EventTypeViewModel>
{
    public EventTypesListSpecification(IEnumerable<int> allowedEventTypeIds = null, bool? includeDetails = null): base(allowedEventTypeIds)
    {
        Query.AsNoTracking();
        Query.Include(x => x.DefaultGroupType);
      
        if (includeDetails.HasValue)
        {
            Query.Include(x => x.Events)
                .ThenInclude(x => x.Church)
                .ThenInclude(x => x.ChurchGroup);
          
            Query.Include(x => x.Events).ThenInclude(x => x.Sessions);
            Query.Include(x => x.Events).ThenInclude(x => x.Schedule);
        }

        Query.Select(x => ExpressionExtensions.SelectEventTypeWithDetails.Compile()(includeDetails ?? false, x));
    }
}