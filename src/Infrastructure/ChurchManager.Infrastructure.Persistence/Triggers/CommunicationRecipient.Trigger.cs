using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Extensions;
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
            if (context.UnmodifiedEntity?.Status.Value != entity.Status.Value)
            {
                // Add recipient to the model
                var person = await personDb.BasicPersonViewModelAsync(entity.PersonId, ct);
                var vm = entity.ToViewModel();
                vm.RecipientPerson = person;
                // push to front-end
                await updater.UpdateRecipientStatusAsync(vm, ct);
            }
        }
    }
}