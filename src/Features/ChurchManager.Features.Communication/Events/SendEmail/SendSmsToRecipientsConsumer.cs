using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Events;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Infrastructure.Abstractions;
using Codeboss.Results;
using Microsoft.Extensions.Logging;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Communication.Events.SendEmail;

public class SendSmsToRecipientsConsumer : IDomainEventHandler
{
    private readonly ICommunicationDbRepository _communicationDb;
    private readonly ITemplateDbRepository _templateDb;
    private readonly ISmsOrchestrator _sms;
    public ILogger<SendSmsToRecipientsEvent> Logger { get; }

    public SendSmsToRecipientsConsumer(
        ICommunicationDbRepository communicationDb,
        ITemplateDbRepository templateDb,
        ILogger<SendSmsToRecipientsEvent> logger,
        ISmsOrchestrator sms)
    {
        _communicationDb = communicationDb;
        _templateDb = templateDb;
        _sms = sms;
        Logger = logger;
    }
    
    public async Task Handle(SendSmsToRecipientsEvent message, CancellationToken ct)
    {
        Logger.LogInformation("✔️------ SendSmsToRecipientsEvent event received ------");

        var communicationId = message.CommunicationId;
        var recipientIds = message.RecipientIds;
        var (content, hasTemplate, recipients, template, isBulk) = await _communicationDb.SmsCommunicationToSendAsync(
            communicationId,
            recipientIds
            );
        
        // ---------------------NON TEMPLATED / BULK SMS---------------------------------
        if (isBulk || !hasTemplate)
        {
            var bulkSmsMessage = new BulkSmsMessage
            {
                Body = content,
                To = recipients
                    .Where(recipient => recipient.Status == CommunicationRecipientStatus.Pending.Value)
                    .Select(recipient => new SmsRecipient
                {
                    PersonId = recipient.PersonId,
                    PhoneNumber = recipient.RecipientPerson!.MessagingPhoneNumber.FullNumber
                }).ToList(),
                DeduplicationId = message.CommunicationId
            };
            
            var operationResult = await _sms.SendBulkSmsAsync(bulkSmsMessage, ct);
            Logger.LogInformation($"SendSmsToRecipient isBulk success: [{operationResult.IsSuccess}] ------");
            
            var recipientResults = operationResult.Result.ToDictionary(r => r.PersonId, r => r);
            foreach (var recipient in recipients)
            {
                recipient.AttemptCount++;
                if (recipientResults.TryGetValue(recipient.PersonId, out var smsResult))
                {
                    recipient.Status = operationResult.IsSuccess && smsResult.IsSent ? CommunicationRecipientStatus.Sent.Value : CommunicationRecipientStatus.Failed.Value;
                    recipient.StatusNote = operationResult.IsSuccess ? null : smsResult.Error;
                    recipient.UniqueMessageId = smsResult.MessageId;
                    recipient.SendDateTime = operationResult.IsSuccess && smsResult.IsSent ? DateTime.UtcNow : null;
                }
                else
                {
                    recipient.Status = CommunicationRecipientStatus.Failed.Value;
                    recipient.StatusNote = "No result received for this recipient";
                    recipient.UniqueMessageId = null;
                    recipient.SendDateTime = null;
                }
            }
            
            await _communicationDb.SaveChangesAsync(ct);
            return;
        }
        
        // ---------------------TEMPLATED SMS---------------------------------
        
        if (hasTemplate)
        {
            var templateInfo = new TemplateInfo(template.Name, null);

            var smsMessages = recipients
                .Where(recipient => recipient.Status == CommunicationRecipientStatus.Pending.Value)
                .Select(recipient => new SmsMessage
            {
                Recipient = new SmsRecipient
                {
                    PersonId = recipient.PersonId,
                    PhoneNumber = recipient.RecipientPerson!.MessagingPhoneNumber.FullNumber
                },
            });
            
            var recipientResults = new Dictionary<int, OperationResult<SmsOperationResult>>();
            await foreach (var result in _sms.SendSmsAsync(smsMessages, templateInfo, ct))
            {
                recipientResults[result.Result.PersonId] = result;
                Logger.LogInformation($"SendSmsToRecipients hasTemplate success: [{result.IsSuccess}] ------");
            }
            
            foreach (var recipient in recipients)
            {
                recipient.AttemptCount++;
                if (recipientResults.TryGetValue(recipient.PersonId, out var operationResult))
                {
                    recipient.Status = operationResult.IsSuccess && operationResult.Result.IsSent ? CommunicationRecipientStatus.Sent.Value : CommunicationRecipientStatus.Failed.Value;
                    recipient.StatusNote = operationResult.IsSuccess ? null : operationResult.Result.Error;
                    recipient.UniqueMessageId = operationResult.Result.MessageId;
                    recipient.SendDateTime = operationResult.IsSuccess && operationResult.Result.IsSent ? DateTime.UtcNow : null;
                }
                else
                {
                    recipient.Status = CommunicationRecipientStatus.Failed.Value;
                    recipient.StatusNote = "No result received for this recipient";
                    recipient.UniqueMessageId = null;
                    recipient.SendDateTime = null;
                }
            }

            await _communicationDb.SaveChangesAsync(ct);
        }
        
    }
}