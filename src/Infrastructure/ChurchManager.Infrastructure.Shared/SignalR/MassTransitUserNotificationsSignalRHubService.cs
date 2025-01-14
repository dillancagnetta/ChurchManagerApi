using ChurchManager.Domain.Features;
using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;
using MassTransit;

namespace ChurchManager.Infrastructure.Shared.SignalR
{
    public class MassTransitUserNotificationsSignalRHubService(IBusControl busControl) :
        MassTransitSignalRHubService<NotificationHub>,
        IUserNotificationsHubService
    {
        public Task SendMessageToUserAsync(Message message, CancellationToken ct = default)
        {
            return SendToUserAsync(
                message, 
                message.UserId.ToString(), 
                "DirectMessage", 
                busControl);
        }
    }
}

