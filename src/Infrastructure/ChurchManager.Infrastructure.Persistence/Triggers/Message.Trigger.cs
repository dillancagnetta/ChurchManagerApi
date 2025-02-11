#region

using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Events;
using ChurchManager.Infrastructure.Abstractions;
using EntityFrameworkCore.Triggered;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Triggers;

/// <summary>
/// Using triggers instead of Domain events because we don't know the Id at that time 
/// </summary>
/// <param name="events"></param>
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