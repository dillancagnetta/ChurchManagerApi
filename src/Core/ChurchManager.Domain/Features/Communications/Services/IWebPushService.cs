namespace ChurchManager.Domain.Features.Communication.Services;

public interface IWebPushService
{
    Task SendNotificationAsync(int personId, PushNotification notification, CancellationToken ct = default);
    Task SendNotificationAsync(string userId, PushNotification notification, CancellationToken ct = default);
}