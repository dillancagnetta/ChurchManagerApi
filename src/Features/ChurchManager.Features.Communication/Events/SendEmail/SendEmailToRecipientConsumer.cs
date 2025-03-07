using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Events;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.Domain.Features.Communications.Services;
using Codeboss.Results;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Communication.Events.SendEmail;

public class SendEmailToRecipientConsumer : IConsumer<SendEmailToRecipientEvent>
{
    private readonly ICommunicationDbRepository _communicationDb;
    private readonly ITemplateDbRepository _templateDb;
    private readonly IEmailOrchestrator _email;
    public ILogger<SendEmailToRecipientConsumer> Logger { get; }

    public SendEmailToRecipientConsumer(
        ICommunicationDbRepository communicationDb,
        ITemplateDbRepository templateDb,
        ILogger<SendEmailToRecipientConsumer> logger,
        IEmailOrchestrator email)
    {
        _communicationDb = communicationDb;
        _templateDb = templateDb;
        _email = email;
        Logger = logger;
    }
    
    public async Task Consume(ConsumeContext<SendEmailToRecipientEvent> context)
    {
        Logger.LogInformation("✔️------ SendEmailToRecipientEvent event received ------");

        var communicationId = context.Message.CommunicationId;
        var recipientId = context.Message.RecipientId;
        var (subject, content, hasTemplate, recipient, template) = await _communicationDb.CommunicationToSendAsync(
            communicationId,
            recipientId
            );

        if (recipient.Status == CommunicationRecipientStatus.Pending.Value)
        {
            var emailRecipient = new EmailRecipient
            {
                EmailAddress = recipient.RecipientPerson.Email.Address,
                PersonId = recipient.PersonId
            };

            var result = new OperationResult<string>();
            if (hasTemplate)
            {
                var templateInfo = new TemplateInfo(template.Name, null);
                result = await _email.SendEmailAsync(emailRecipient, subject, templateInfo, context.CancellationToken);
            }
            else
            {
                result = await _email.SendEmailAsync(emailRecipient, subject, content, context.CancellationToken);
            }
            Logger.LogInformation($"SendEmailAsync success: [{result.IsSuccess}] ------");
            
            recipient.AttemptCount++;
            recipient.Status = result.IsSuccess? CommunicationRecipientStatus.Sent : CommunicationRecipientStatus.Failed.Value;
            recipient.StatusNote = result.IsSuccess? null : result.Errors.First().Message;
            recipient.UniqueMessageId = result.IsSuccess? result.Result : null;
            recipient.SendDateTime = result.IsSuccess? DateTime.UtcNow : null;
            _communicationDb.SaveChangesAsync();
        }
    }
}