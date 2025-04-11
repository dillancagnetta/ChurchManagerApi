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

    public async Task<(string Subject, string Content, bool HasTemplate, CommunicationRecipient Recipient, CommunicationTemplate Template)> CommunicationToSendAsync(int communicationId, int recipientId, CancellationToken ct = default)
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
                Content = x.CommunicationContent,
                HasTemplate = x.IsTemplatedCommunication(),
                Recipient = x.Recipients.SingleOrDefault(r => r.Id == recipientId),
                Template = x.CommunicationTemplate
            }).SingleOrDefaultAsync(ct);
        
        return (communication.Subject, communication.Content, communication.HasTemplate, communication.Recipient, communication.Template);
    }

    public async Task<(string Content, bool HasTemplate, IList<CommunicationRecipient> Recipients, CommunicationTemplate Template, bool IsBulk)>
        SmsCommunicationToSendAsync(int communicationId, IList<int> recipientIds, CancellationToken ct = default)
    {
        var communication = await Queryable()
            .Include(x => x.Recipients)
            //.Include(x => x.Attachments)
                .ThenInclude(r => r.RecipientPerson)
                    .ThenInclude(r => r.PhoneNumbers)
            .Include(x => x.CommunicationTemplate)
            .Where(x => x.Id == communicationId)
            .Select(x => new
            {
                //Attachments = x.Attachments,
                //Subject = x.Subject,
                Content = x.CommunicationContent,
                HasTemplate = x.IsTemplatedCommunication(),
                Recipients = x.Recipients.Where(r => recipientIds.Contains(r.Id)).ToList(),
                Template = x.CommunicationTemplate,
                IsBulk = x.IsBulkCommunication
            }).SingleOrDefaultAsync(ct);
        
        return (communication.Content, communication.HasTemplate, communication.Recipients, communication.Template, communication.IsBulk);
        
    }
}