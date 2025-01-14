using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Events;
using ChurchManager.Domain.Features.Communication.Repositories;
using ChurchManager.Domain.Features.Communication.Services;
using Codeboss.Types;
using MassTransit;

namespace ChurchManager.Features.Communication.Events.MessageEvents;

public class SendUserMessageConsumer: IConsumer<MessageForUserAddedEvent>
{
    private readonly IMessageDbRepository _dbRepository;
    private readonly IDateTimeProvider _dateTime;
    private readonly IMessageSender _sender;

    public SendUserMessageConsumer(
        IMessageDbRepository dbRepository,
        IDateTimeProvider dateTime,
        IMessageSender sender)
    {
        _dbRepository = dbRepository;
        _dateTime = dateTime;
        _sender = sender;
    }
    
    public async Task Consume(ConsumeContext<MessageForUserAddedEvent> context)
    {
        var messageId = context.Message.MessageId;
        
        var message = await _dbRepository.GetByIdAsync(messageId, context.CancellationToken);

        if (message.Status == MessageStatus.Pending.Value)
        {
            await _sender.SendAsync(message, context.CancellationToken);
            message.MarkAsSent(_dateTime);
            await _dbRepository.SaveChangesAsync();
        }
    }
}