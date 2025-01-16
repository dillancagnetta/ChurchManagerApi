namespace ChurchManager.Domain.Features.Communication.Services
{
    public interface IWebPushSenderClient
    {
        Task SendNotificationAsync(PushDevice device, PushNotification notification, CancellationToken ct = default);
    }
}
