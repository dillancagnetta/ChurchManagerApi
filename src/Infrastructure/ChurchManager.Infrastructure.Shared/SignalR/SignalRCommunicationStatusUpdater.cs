using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Extensions;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;
using Wolverine;

namespace ChurchManager.Infrastructure.Shared.SignalR;

public class SignalRCommunicationStatusUpdater(IMessageBus bus) : ICommunicationStatusUpdater
{
    public async ValueTask UpdateRecipientStatusAsync(
        CommunicationRecipientViewModel recipient, 
        CancellationToken ct = default)
    {
        // Create message for broadcasting to all clients
        var broadcastMessage = new SignalRBroadcastMessage
        {
            HubType = typeof(CommunicationStatusHub),
            MethodName = "ReceiveRecipientStatusUpdate",
            Payload = recipient
        };    
        await bus.PublishAsync(broadcastMessage);
    }

    public async ValueTask UpdateBatchRecipientStatusAsync(
        CommunicationRecipient[] recipients, 
        CancellationToken ct = default)
    {
        // Create message for broadcasting to all clients
        var broadcastMessage = new SignalRBroadcastMessage
        {
            HubType = typeof(CommunicationStatusHub),
            MethodName = "ReceiveBatchRecipientStatusUpdate",
            Payload = recipients
        };    
        await bus.PublishAsync(broadcastMessage);
    }

    public async ValueTask UpdateStatusAsync(
        Communication communication, 
        CancellationToken ct = default)
    {
        // Create message for broadcasting to all clients
        var broadcastMessage = new SignalRBroadcastMessage
        {
            HubType = typeof(CommunicationStatusHub),
            MethodName = "ReceiveCommunicationStatusUpdate",
            Payload = communication
        };    
        await bus.PublishAsync(broadcastMessage);
    }

    public async ValueTask UpdateProgressAsync(int communicationId, int processed, int total, CancellationToken ct = default)
    {
        // Create message for broadcasting to all clients
        var broadcastMessage = new SignalRBroadcastMessage
        {
            HubType = typeof(CommunicationStatusHub),
            MethodName = "ReceiveCommunicationProgressUpdate",
            Payload = new { communicationId, processed, total }
        };    
        await bus.PublishAsync(broadcastMessage);
    }
}