using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class CommunicationDbRepository : GenericRepositoryBase<Communication>, ICommunicationDbRepository
{
    public CommunicationDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(string Subject, CommunicationRecipient Recipient, CommunicationTemplate Template)> CommunicationToSendAsync(int communicationId, int recipientId, CancellationToken ct = default)
    {
        var communication = await Queryable()
                .Include(x => x.Recipients)
                //.Include(x => x.Attachments)
                    .ThenInclude(r => r.RecipientPerson)
                .Include(x => x.CommunicationTemplate)
            .Where(x => x.Id == communicationId)
            .Select(x => new
            {
                //Attachments = x.Attachments,
                Subject = x.Subject,
                Recipient = x.Recipients.SingleOrDefault(r => r.Id == recipientId),
                Template = x.CommunicationTemplate
            }).SingleOrDefaultAsync(ct);
        
        return (communication.Subject, communication.Recipient, communication.Template);
    }
}