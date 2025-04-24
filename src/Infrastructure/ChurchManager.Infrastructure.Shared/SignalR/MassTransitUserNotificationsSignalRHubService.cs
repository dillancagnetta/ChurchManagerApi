/*
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Services;
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
                "UserMessage", 
                busControl, ct);
        }
    }
}
*/

using System.Text.Json;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Wolverine;

namespace ChurchManager.Infrastructure.Shared.SignalR
{
    public class WolverineUserNotificationsSignalRHubService : IUserNotificationsHubService
    {
        private readonly IMessageBus _bus;
        private readonly IHubContext<NotificationHub> _hubContext;

        public WolverineUserNotificationsSignalRHubService(
            IMessageBus bus, 
            IHubContext<NotificationHub> hubContext)
        {
            _bus = bus;
            _hubContext = hubContext;
        }

        public ValueTask SendMessageToUserAsync(Message message, CancellationToken ct = default)
        {
            // Create a SignalR notification message
            var signalRMessage = new SignalRUserMessage
            {
                UserId = message.UserId.ToString(),
                MethodName = "UserMessage",
                Payload = message
            };

            // Publish through Wolverine
            return _bus.PublishAsync(signalRMessage);
        }

        public ValueTask SendToUserAsync<TModel>(TModel model, string userId, string methodName,
            CancellationToken ct = default)
        {
            var signalRMessage = new SignalRUserMessage
            {
                UserId = userId,
                MethodName = methodName,
                Payload = JsonSerializer.Serialize(model)
            };
            
            return _bus.PublishAsync(signalRMessage);
        }
    }

    // Message class for SignalR notifications
    public class SignalRUserMessage
    {
        public string UserId { get; set; }
        public string MethodName { get; set; }
        public object Payload { get; set; }
    }

    // Handler for the SignalR messages
    public class SignalRUserMessageHandler
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRUserMessageHandler(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task Handle(SignalRUserMessage message, CancellationToken ct)
        {
            // Send to the specified user through SignalR
            return _hubContext.Clients.User(message.UserId)
                .SendAsync(message.MethodName, message.Payload, ct);
        }
    }
}
