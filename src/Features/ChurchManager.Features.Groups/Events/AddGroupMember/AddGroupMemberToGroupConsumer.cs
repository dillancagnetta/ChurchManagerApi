using ChurchManager.Domain.Features.Events.DomainEvents;
using ChurchManager.Domain.Features.Groups.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Groups.Events.AddGroupMember;

public class AddGroupMemberToGroupConsumer(
    IGroupMemberDbRepository groupMemberDb,
    ILogger<AddGroupMemberToGroupConsumer> logger) : IConsumer<PersonRegisteredForEvent>
{
    public async Task Consume(ConsumeContext<PersonRegisteredForEvent> context)
    {
        logger.LogInformation("✔️ ------ PersonRegisteredForEvent event received ------");
        
        var message = context.Message;
        
        await groupMemberDb.AddGroupMember(
            message.GroupId, 
            message.PersonId,
            message.GroupRoleId, 
            ct: context.CancellationToken);
    }
}