using ChurchManager.Infrastructure.Abstractions.Persistence;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Communication.Repositories;

public interface IMessageDbRepository: IGenericDbRepository<Message>
{
    Task<IEnumerable<Message>> AllAsync(Guid userLoginId, IPagedQuery paging = null, CancellationToken ct = default);
    Task<int> UnreadCountAsync(Guid userLoginId, CancellationToken ct = default);
    Task MarkAsReadAsync(int messageId, CancellationToken ct = default);
    Task DeleteAsync(int messageId, CancellationToken ct = default);
    Task MarkAllAsReadAsync(Guid userLoginId, CancellationToken ct = default);
    Task DeleteAllAsync(Guid userLoginId, CancellationToken ct = default);
}