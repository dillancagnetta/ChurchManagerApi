using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.Abstractions.Services;

public interface IEventTypeService: ICrudServiceAsync<EventType, EventTypeViewModel, EditEventTypeModel>
{
    Task<IReadOnlyList<EventTypeViewModel>> AllEventsAsync(int? eventTypeId = null, bool? includeDetails = null, CancellationToken ct = default);
}

public interface IEventService: ICrudServiceAsync<Event, EventViewModel, EditEventViewModel>
{
}