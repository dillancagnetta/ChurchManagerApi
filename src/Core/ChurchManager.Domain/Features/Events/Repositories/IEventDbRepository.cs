using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Events.Repositories;

public interface IEventDbRepository: IGenericDbRepository<Event>
{
    Task<EventViewModel> EventDetailsAsync(int eventId, CancellationToken ct = default);
}