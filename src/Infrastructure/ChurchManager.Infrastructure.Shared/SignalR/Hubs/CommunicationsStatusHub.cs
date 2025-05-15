using ChurchManager.Domain.Features.Communications;
using ChurchManager.Infrastructure.Abstractions.SignalR;
using Codeboss.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Shared.SignalR.Hubs;

[Authorize]
public class CommunicationStatusHub(
    IDateTimeProvider dateTime,
    IConnectionTracker tracker,
    ILogger<CommunicationStatusHub> logger): Hub
{
    #region Overrides

    public override async Task OnConnectedAsync()
    {
        logger.LogDebug("[√] CommunicationStatusHub Connected for {user}", Context.UserIdentifier);
        
        // Add user connections to a group for that user
        var connectionId = Context.ConnectionId;
        var userId = Context.UserIdentifier;
        await Groups.AddToGroupAsync(connectionId, userId);
        await tracker.AddConnectionAsync(userId, connectionId);

        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception ex)
    {
        var connectionId = Context.ConnectionId;
        var userId = Context.UserIdentifier;
        // Remove user connection to a group for that user
        await Groups.RemoveFromGroupAsync(connectionId, userId);
        await tracker.RemoveConnectionAsync(userId, connectionId);

        await base.OnDisconnectedAsync(ex);
    }

    #endregion
    
    #region Methods

    // Method to update message status for a specific message
    public async Task UpdateRecipientStatus(int communicationRecipientId, CommunicationRecipientStatus status)
    {
        // Send update to all connected clients
        await Clients.All.SendAsync("ReceiveRecipientStatusUpdate", communicationRecipientId, status.Value);
    }

    // Method to update message status for multiple messages at once
    public async Task UpdateBatchMessageStatus(string[] messageIds, string status)
    {
        await Clients.All.SendAsync("ReceiveBatchStatusUpdate", messageIds, status);
    }

    // Method to broadcast progress update about the overall process
    public async Task UpdateBulkProgress(int processed, int total, string batchId)
    {
        await Clients.All.SendAsync("ReceiveProgressUpdate", processed, total, batchId);
    }

    #endregion
}