using ChurchManager.Application.Abstractions.Services;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Events.Queries.GetEvents;

public record EventsQuery(int? EventTypeId = null, bool? IncludeDetails = null): IRequest<ApiResponse>;

public class AddChurchGroupCommandHandler(IEventTypeService service) : IRequestHandler<EventsQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(EventsQuery query, CancellationToken ct)
    {
        var events = await service.AllEventsAsync(query.EventTypeId, query.IncludeDetails, ct);
      
        return new ApiResponse(events);
    }
}
