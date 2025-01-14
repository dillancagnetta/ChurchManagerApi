using ChurchManager.Domain.Features;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Events;
using ChurchManager.Infrastructure.Abstractions;
using EntityFrameworkCore.Triggered;

namespace ChurchManager.Infrastructure.Persistence.Triggers;

public class MessageTrigger(IDomainEventPublisher events): IAfterSaveTrigger<Message> 
{
    public async Task AfterSave(ITriggerContext<Message> context, CancellationToken ct)
    {
        if (context.ChangeType == ChangeType.Added)
        {
            var message = context.Entity;
        
            if (message.Status == MessageStatus.Pending)
            {
                await events.PublishAsync(new MessageForUserAddedEvent(message.Id), ct);
            }
        }
    }
}