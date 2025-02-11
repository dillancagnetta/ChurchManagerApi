namespace ChurchManager.Domain.Features.Communications.Services
{
    public interface IWebPushSenderClient
    {
        Task SendNotificationAsync(PushDevice device, PushNotification notification, CancellationToken ct = default);
    }
}
