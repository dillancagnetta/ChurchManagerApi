using ChurchManager.Domain.Features.Events.DomainEvents;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Groups.Events.AddGroupMember;

public class AddGroupMemberToGroupConsumer(
    IGroupMemberDbRepository groupMemberDb,
    ILogger<AddGroupMemberToGroupConsumer> logger): IDomainEventHandler
{
    public async Task Handle(PersonRegisteredForEvent message, CancellationToken ct)
    {
        logger.LogInformation("✔️ ------ PersonRegisteredForEvent event received ------");
        
        
        await groupMemberDb.AddGroupMember(
            message.GroupId, 
            message.PersonId,
            message.GroupRoleId, 
            ct: ct);
    }
}