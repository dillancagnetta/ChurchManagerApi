using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Domain.Features.People.Repositories;
using EntityFrameworkCore.Triggered;

namespace ChurchManager.Infrastructure.Persistence.Triggers;

public class CommunicationRecipientTrigger(
    ICommunicationStatusUpdater updater,
    IPersonDbRepository personDb): IAfterSaveTrigger<CommunicationRecipient>
{
    public async Task AfterSave(ITriggerContext<CommunicationRecipient> context, CancellationToken ct)
    {
        if (context.ChangeType == ChangeType.Modified)
        {
            var entity = context.Entity;

            // Status changed
            if (context.UnmodifiedEntity?.Status.Value != entity.Status.Value && entity.AttemptCount > 0)
            {
                var person = await personDb.GetByIdAsync(entity.PersonId, ct);
                entity.RecipientPerson = person;
                await updater.UpdateRecipientStatusAsync(entity, ct);
            }
        }
    }
}