using ChurchManager.Domain.Features.Communication.Repositories;
using ChurchManager.Domain.Features.Communication.Services;
using CodeBoss.Jobs.Abstractions;
using CodeBoss.Jobs.Jobs;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Domain.Features.Communication.Jobs;
/// <summary>
/// NOT YET REGISTERED
/// </summary>
public class SendMessagesJob(
    IServiceJobRepository repository, 
    IMessageDbRepository messageDb,
    IMessageSender sender,
    ILogger<CodeBossJob> logger) : CodeBossJob(repository, logger)
{
    public override async Task Execute(CancellationToken ct = new CancellationToken())
    {
        var messages = await messageDb.AllPendingMessagesAsync(ct);

        foreach (var message in messages)
        {
            await sender.SendAsync(message, ct);
            await messageDb.MarkAsReadAsync(message.Id, ct);
        }
    }
}