/*using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;
using Codeboss.Types;
using MassTransit;
using MassTransit.SignalR.Contracts;
using MassTransit.SignalR.Utils;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace ChurchManager.Infrastructure.Shared.SignalR
{
    public class MassTransitSignalRPushNotificationsService : IPushNotificationsService
    {
        private readonly IBusControl _busControl;
        private readonly IReadOnlyList<IHubProtocol> _protocols = new IHubProtocol[] { new JsonHubProtocol() };

        public MassTransitSignalRPushNotificationsService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task PushAsync(INotification notification, CancellationToken token = default)
        {
            switch (notification.Scope)
            {
                case Constants.Notifications.Scope.All:
                {
                    await _busControl.Publish<All<NotificationHub>>(new
                        {
                            Messages = _protocols.ToProtocolDictionary(notification.MethodName, new object[] { notification })
                        }, token)
                        .ConfigureAwait(false);
                    break;
                }
                case Constants.Notifications.Scope.User:
                {
                    await _busControl.Publish<User<NotificationHub>>(new
                        {
                            notification.UserId,
                            Messages = _protocols.ToProtocolDictionary(notification.MethodName, new object[] { notification })
                        }, token)
                        .ConfigureAwait(false);
                    break;
                }
            }
        }
    }
}*/

using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;
using Codeboss.Types;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wolverine;

namespace ChurchManager.Infrastructure.Shared.SignalR
{
    public class WolverineSignalRPushNotificationsService : IPushNotificationsService
    {
        private readonly IMessageBus _bus;
        private readonly IReadOnlyList<IHubProtocol> _protocols = new IHubProtocol[] { new JsonHubProtocol() };

        public WolverineSignalRPushNotificationsService(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task PushAsync(INotification notification, CancellationToken token = default)
        {
            switch (notification.Scope)
            {
                case Constants.Notifications.Scope.All:
                {
                    // Create message for broadcasting to all clients
                    var broadcastMessage = new SignalRBroadcastMessage
                    {
                        HubType = typeof(NotificationHub),
                        MethodName = notification.MethodName,
                        Payload = notification
                    };
                    
                    await _bus.PublishAsync(broadcastMessage);
                    break;
                }
                case Constants.Notifications.Scope.User:
                {
                    // Create message for specific user
                    var userMessage = new SignalRUserMessage
                    {
                        UserId = notification.UserId,
                        MethodName = notification.MethodName,
                        Payload = notification
                    };
                    
                    await _bus.PublishAsync(userMessage);
                    break;
                }
            }
        }
    }


}