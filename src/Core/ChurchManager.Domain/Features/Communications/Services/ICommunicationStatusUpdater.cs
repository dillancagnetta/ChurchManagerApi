namespace ChurchManager.Domain.Features.Communications.Services;

public interface ICommunicationStatusUpdater
{
    ValueTask  UpdateRecipientStatusAsync(
        CommunicationRecipient recipient, 
        CancellationToken ct = default);
    
    ValueTask  UpdateBatchRecipientStatusAsync(
        CommunicationRecipient[] recipients, 
        CancellationToken ct = default);
    
    ValueTask  UpdateStatusAsync(
        Communication communication, 
        CancellationToken ct = default);

    ValueTask UpdateProgressAsync(
        int communicationId,
        int processed, int total, CancellationToken ct = default);
}