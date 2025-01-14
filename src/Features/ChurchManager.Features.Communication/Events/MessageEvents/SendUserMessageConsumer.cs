using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Events;
using ChurchManager.Domain.Features.Communication.Repositories;
using ChurchManager.Domain.Features.Communication.Services;
using MassTransit;

namespace ChurchManager.Features.Communication.Events.MessageEvents;

public class SendUserMessageConsumer: IConsumer<MessageForUserAddedEvent>
{
    private readonly IMessageDbRepository _dbRepository;
    private readonly IMessageSender _sender;

    public SendUserMessageConsumer(
        IMessageDbRepository dbRepository,
        IMessageSender sender)
    {
        _dbRepository = dbRepository;
        _sender = sender;
    }
    
    public async Task Consume(ConsumeContext<MessageForUserAddedEvent> context)
    {
        var messageId = context.Message.MessageId;
        
        var message = await _dbRepository.GetByIdAsync(messageId, context.CancellationToken);

        if (message.Status == MessageStatus.Pending)
        {
            await _sender.SendAsync(message, context.CancellationToken);
            message.Status = MessageStatus.Sent;
            await _dbRepository.SaveChangesAsync();
        }
    }
}