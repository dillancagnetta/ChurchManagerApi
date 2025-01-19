using ChurchManager.Infrastructure.Abstractions.MassTransit;

namespace ChurchManager.Domain.Features.Communication.Services
{
    public interface IUserNotificationsHubService : IUserHubService
    {
        Task SendMessageToUserAsync(Message message, CancellationToken ct = default);
    }
}