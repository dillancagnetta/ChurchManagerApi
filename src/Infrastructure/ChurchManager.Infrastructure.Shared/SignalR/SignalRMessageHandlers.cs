using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChurchManager.Infrastructure.Shared.SignalR;

// Message types for Wolverine
public class SignalRBroadcastMessage
{
    public Type HubType { get; set; }
    public string MethodName { get; set; }
    public object Payload { get; set; }
}
    

// Handlers for SignalR messages

public class SignalRMessageHandler : IDomainEventHandler
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IHubContext<CommunicationStatusHub> _communicationsHub;

    public SignalRMessageHandler(
        IHubContext<NotificationHub> hubContext,
        IHubContext<CommunicationStatusHub> communicationsHub
    )
    {
        _hubContext = hubContext;
        _communicationsHub = communicationsHub;
    }

    public Task Handle(SignalRBroadcastMessage message, CancellationToken ct)
    {
        // Send to all clients
        if (message.HubType == typeof(NotificationHub))
        {
            return _hubContext.Clients.All.SendAsync(
                message.MethodName, 
                message.Payload, 
                ct);
        } else if (message.HubType == typeof(CommunicationStatusHub))
        {
            return _communicationsHub.Clients.All.SendAsync(
                message.MethodName, 
                message.Payload, 
                ct);
        }
        return Task.CompletedTask;
    }

    /*public Task Handle(SignalRUserMessage message, CancellationToken ct)
        {
            // Send to specific user
            return _hubContext.Clients.User(message.UserId).SendAsync(
                message.MethodName, 
                new[] { message.Payload }, 
                ct);
        }*/
}