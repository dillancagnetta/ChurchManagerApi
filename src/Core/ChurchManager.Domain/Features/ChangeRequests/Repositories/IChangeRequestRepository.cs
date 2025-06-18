using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.ChangeRequests.Repositories;

public interface IChangeRequestRepository: IGenericDbRepository<ChangeRequest>
{
    Task<IList<ChangeRequest>> AllPendingRequestsAsync(bool includeProperties = false, CancellationToken ct = default);
    Task<IList<ChangeRequest>> RequestsForEntityAsync(string entityType, int entityId, CancellationToken ct= default);
    Task<IList<ChangeRequest>> RequestsByPersonAsync(int personId, CancellationToken ct= default);
}