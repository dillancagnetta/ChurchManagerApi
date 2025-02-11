using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Events;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Communication.Events.CommunicationEvents;

public class CommunicationApprovedConsumer: IConsumer<CommunicationApprovedEvent>
{
    private readonly IGenericDbRepository<Domain.Features.Communications.Communication> _dbRepository;
    private readonly IPersonDbRepository _peopleDb;
    public ILogger<CommunicationApprovedConsumer> Logger { get; }
    
    private readonly Predicate<Email> _isEmailActive = e => e?.IsActive != null && e.IsActive.Value;

    public CommunicationApprovedConsumer(
        IGenericDbRepository<Domain.Features.Communications.Communication> dbRepository,
        IPersonDbRepository peopleDb,
        ILogger<CommunicationApprovedConsumer> logger)
    {
        _dbRepository = dbRepository;
        _peopleDb = peopleDb;
        Logger = logger;
    }
    
    public async Task Consume(ConsumeContext<CommunicationApprovedEvent> context)
    {
        Logger.LogInformation("✔️------ CommunicationApprovedEvent event received ------");
        
        var communication = await _dbRepository
            .Queryable()
                .Include(x => x.Recipients)
                .Include(x => x.Attachments)
                .Include(x => x.CommunicationTemplate)
            .FirstOrDefaultAsync(c => c.Id == context.Message.CommunicationId, context.CancellationToken);
        
        if (communication is not null && communication.Status == CommunicationStatus.Approved.Value)
        {
            if (communication.FutureSendDateTime.HasValue)
            {
                Uri _queue = new Uri("queue:schedule-communication");

                await context.ScheduleSend<CommunicationScheduledEvent>(_queue,
                    communication.FutureSendDateTime.Value, new 
                    {
                        CommunicationId = context.Message.CommunicationId
                    }, context.CancellationToken);
            }
            else
            {
                // Email
                if (communication.CommunicationType == CommunicationType.Email.Value)
                {
                    if (communication.IsBulkCommunication)
                    {
                
                    }
                    else
                    {
                        var recipients = communication.Recipients.Where(x => x.Status == CommunicationRecipientStatus.Pending.Value);
                        var recipientPersonIds = recipients.Select(x => x.PersonId).ToList();
                        var people = await _peopleDb.Queryable()
                            .AsNoTracking()
                            .Where(x => recipientPersonIds.Contains(x.Id))
                            .Select(x => new { x.Id, x.Email })
                            .ToListAsync(context.CancellationToken);
                        
                        var peopleWithActiveEmail = people.Where(x => _isEmailActive(x.Email));

                        // Set failure status for recipients without active email addresses
                        var peopleWithoutActiveEmails = recipientPersonIds.Except(peopleWithActiveEmail.Select(x => x.Id));
                        foreach (var personWithoutActiveEmail in peopleWithoutActiveEmails)
                        {
                            var recipient = recipients.FirstOrDefault(x => x.PersonId == personWithoutActiveEmail);
                            recipient.Status = CommunicationRecipientStatus.Failed.Value;
                            recipient.StatusNote = "Email address not found or is not active.";
                        }

                        // Send to recipients with active email addresses
                        foreach (var personWithActiveEmail in peopleWithActiveEmail)
                        {
                            var recipient = recipients.FirstOrDefault(x => x.PersonId == personWithActiveEmail.Id);
                            
                            await context.Publish(new SendEmailToRecipientEvent(
                                communication.Id,
                                recipient.Id
                            ), context.CancellationToken);
                        }
                        
                        // Save changes
                        communication.SendDateTime = DateTime.UtcNow;
                        _dbRepository.SaveChangesAsync();
                    }  
                }
                
                // SMS
                if (communication.CommunicationType == CommunicationType.SMS.Value)
                {
                    
                }
            }
        }
    }
}