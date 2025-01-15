using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Events;
using ChurchManager.Domain.Features.Communication.Repositories;
using ChurchManager.Domain.Features.Communication.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Communication.Events.MessageEvents;

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
        Logger.LogInformation($"📩 [{nameof(MessageForUserAddedEvent)}] Message Received: {context.Message.MessageId}");

        var messageId = context.Message.MessageId;
        
        var message = await _dbRepository.GetByIdAsync(messageId, context.CancellationToken);

        if (message.Status == MessageStatus.Pending.Value)
        {
            await _sender.SendAsync(message, context.CancellationToken);
            Logger.LogInformation($"🚀 [{nameof(MessageForUserAddedEvent)}] Message Sent: {context.Message.MessageId}");
        }
    }
}