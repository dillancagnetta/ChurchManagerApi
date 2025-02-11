using ChurchManager.Domain.Features.Communications;

namespace ChurchManager.Application.Abstractions.Services
{
    public interface IPushNotificationService
    {
        Task SendNotificationToPersonAsync(int personId, PushNotification notification, CancellationToken cancellationToken = default);
    }
}
