using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Events;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace ChurchManager.Features.Communication.Events.CommunicationEvents;

public class CommunicationApprovedConsumer: IDomainEventHandler
{
    private readonly IGenericDbRepository<Domain.Features.Communications.Communication> _dbRepository;
    private readonly IPersonDbRepository _peopleDb;
    public ILogger<CommunicationApprovedConsumer> Logger { get; }
    
    private readonly Predicate<Email?> _isEmailActive = e => e?.IsActive != null && e.IsActive.Value;

    public CommunicationApprovedConsumer(
        IGenericDbRepository<Domain.Features.Communications.Communication> dbRepository,
        IPersonDbRepository peopleDb,
        ILogger<CommunicationApprovedConsumer> logger)
    {
        _dbRepository = dbRepository;
        _peopleDb = peopleDb;
        Logger = logger;
    }
    
    public async Task Handle(CommunicationApprovedEvent message, IMessageContext context, CancellationToken ct)
    {
        Logger.LogInformation("✔️------ CommunicationApprovedEvent event received ------");
        
        var communication = await _dbRepository
            .Queryable()
                .Include(x => x.Recipients)
                .Include(x => x.Attachments)
                .Include(x => x.CommunicationTemplate)
            .FirstOrDefaultAsync(c => c.Id == message.CommunicationId, ct);
        
        if (communication is not null && communication.Status == CommunicationStatus.Approved.Value)
        {
            if (communication.FutureSendDateTime.HasValue)
            {
                var dt = communication.FutureSendDateTime.Value;
                await context.ScheduleAsync<CommunicationScheduledEvent>(
                    new CommunicationScheduledEvent(message.CommunicationId),
                    new DateTimeOffset(dt) 
                    );
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
                        var recipients = communication.Recipients.Where(x => x.Status == CommunicationRecipientStatus.Pending.Value)
                            .ToList();
                        var recipientPersonIds = recipients.Select(x => x.PersonId).ToList();
                        var people = await _peopleDb.Queryable()
                            .AsNoTracking()
                            .Where(x => recipientPersonIds.Contains(x.Id))
                            .Select(x => new { x.Id, x.Email })
                            .ToListAsync(ct);
                        
                        var peopleWithActiveEmail = people.Where(x => _isEmailActive(x.Email)).ToList();

                        // Set failure status for recipients without active email addresses
                        var peopleWithoutActiveEmails = recipientPersonIds.Except(peopleWithActiveEmail.Select(x => x.Id));
                        foreach (var personWithoutActiveEmail in peopleWithoutActiveEmails)
                        {
                            var recipient = recipients.FirstOrDefault(x => x.PersonId == personWithoutActiveEmail)!;
                            recipient.Status = CommunicationRecipientStatus.Failed.Value;
                            recipient.StatusNote = "Email address not found or is not active.";
                        }

                        // Send to recipients with active email addresses
                        foreach (var personWithActiveEmail in peopleWithActiveEmail)
                        {
                            var recipient = recipients.First(x => x.PersonId == personWithActiveEmail.Id);
                            
                            await context.PublishAsync(new SendEmailToRecipientEvent(
                                communication.Id,
                                recipient.Id
                            ));
                        }
                        
                        // Save changes
                        communication.SendDateTime = DateTime.UtcNow;
                        await _dbRepository.SaveChangesAsync(ct);
                    }  
                }
                
                // SMS
                if (communication.CommunicationType == CommunicationType.SMS.Value)
                {
                    var recipients = communication.Recipients.Where(x => x.Status == CommunicationRecipientStatus.Pending.Value)
                        .ToList();
                    var recipientPersonIds = recipients.Select(x => x.PersonId).ToList();
                    var people = await _peopleDb.Queryable()
                        .Include(x => x.PhoneNumbers)
                        .AsNoTracking()
                        .Where(x => recipientPersonIds.Contains(x.Id))
                        .Select(x => new { x.Id, PhoneNumber = x.PhoneNumbers.FirstOrDefault(x => x.IsMessagingEnabled) })
                        .ToListAsync(ct);
                    
                    var peopleWithActiveSms = people.Where(x => x.PhoneNumber != null).ToList();

                    // Set failure status for recipients without active email addresses
                    var peopleWithoutActiveSms = recipientPersonIds.Except(peopleWithActiveSms.Select(x => x.Id));
                    foreach (var personWithoutActiveEmail in peopleWithoutActiveSms)
                    {
                        var recipient = recipients.FirstOrDefault(x => x.PersonId == personWithoutActiveEmail)!;
                        recipient.Status = CommunicationRecipientStatus.Failed.Value;
                        recipient.StatusNote = "Phone number not found that is messaging enabled.";
                    }
                                            
                    // Save changes
                    communication.SendDateTime = DateTime.UtcNow;
                    await _dbRepository.SaveChangesAsync(ct);
                    
                    // Send to recipients with active sms phone numbers
                    var activeRecipients = recipients.Where(
                        x => peopleWithActiveSms.Select(x => x.Id).Contains(x.PersonId));
                    await context.PublishAsync(new SendSmsToRecipientsEvent(
                        communication.Id,
                        RecipientIds:activeRecipients.Select(x => x.Id).ToArray()
                    ));
                }
            }
        }
    }
}