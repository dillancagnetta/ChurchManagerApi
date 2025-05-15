using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChurchManager.Infrastructure.Shared.SignalR;

// Message class for SignalR notifications
public class SignalRUserMessage
{
    public string UserId { get; set; }
    public string MethodName { get; set; }
    public object Payload { get; set; }
}

// Handler for the SignalR messages
public class SignalRUserMessageHandler : IDomainEventHandler  
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