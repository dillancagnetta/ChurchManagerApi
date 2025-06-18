using Codeboss.Types;

namespace ChurchManager.Domain.Features.Communications.Services
{
    // Marker for easy access
    public interface IPushNotificationsService
    {
        Task PushAsync(INotification notification, CancellationToken token = default);
    }
}
