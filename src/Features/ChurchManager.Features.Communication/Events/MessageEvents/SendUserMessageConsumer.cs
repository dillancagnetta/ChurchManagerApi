using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Events;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.Domain.Features.Communications.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Communication.Events.MessageEvents;

/// <summary>
/// Consumes events triggered when adding a new message to the database.
/// </summary>
public class SendUserMessageConsumer: IConsumer<MessageForUserAddedEvent>
{
    public ILogger<SendUserMessageConsumer> Logger { get; }
    private readonly IMessageDbRepository _dbRepository;
    private readonly IMessageSender _sender;

    public SendUserMessageConsumer(
        IMessageDbRepository dbRepository,
        IMessageSender sender,
        ILogger<SendUserMessageConsumer> logger)
    {
        Logger = logger;
        _dbRepository = dbRepository;
        _sender = sender;
    }
    
    public async Task Consume(ConsumeContext<MessageForUserAddedEvent> context)
    {
        Logger.LogInformation($"✔️ [{nameof(MessageForUserAddedEvent)}] Message Received: {context.Message.MessageId}");

        var messageId = context.Message.MessageId;
        
        var message = await _dbRepository.GetByIdAsync(messageId, context.CancellationToken);

        // Only send the message if it is pending and not already sent.
        if (message.Status == MessageStatus.Pending.Value)
        {
            await _sender.SendAsync(message, context.CancellationToken);
            Logger.LogInformation($"*** [{nameof(MessageForUserAddedEvent)}] Message Sent: {context.Message.MessageId}");
        }
    }
}