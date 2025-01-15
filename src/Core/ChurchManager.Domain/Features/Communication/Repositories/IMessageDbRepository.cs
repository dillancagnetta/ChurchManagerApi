using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Communication.Repositories;

public interface IMessageDbRepository: IGenericDbRepository<Message>
{
    Task<IList<MessageViewModel>> AllAsync(Guid userLoginId, MessageStatus status, IPagedQuery paging = null,CancellationToken ct = default);
    Task<int> UnreadCountAsync(Guid userLoginId, CancellationToken ct = default);
    Task MarkAsReadAsync(int messageId, CancellationToken ct = default);
    Task DeleteAsync(int messageId, CancellationToken ct = default);
    Task MarkAllAsReadAsync(Guid userLoginId, CancellationToken ct = default);
    Task DeleteAllAsync(Guid userLoginId, CancellationToken ct = default);
    Task<IList<Message>> AllPendingMessagesAsync(CancellationToken ct);
}