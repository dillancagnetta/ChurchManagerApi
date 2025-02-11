using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Communications.Repositories;

public interface ICommunicationDbRepository : IGenericDbRepository<Communication>
{
    Task<(string Subject, string Content, bool HasTemplate, CommunicationRecipient Recipient, CommunicationTemplate Template)>
        CommunicationToSendAsync(int communicationId, int recipientId, CancellationToken ct = default);
}   