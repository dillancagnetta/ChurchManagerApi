using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.Domain.Features.Communications.Services;
using CodeBoss.Jobs.Abstractions;
using CodeBoss.Jobs.Jobs;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Domain.Features.Communications.Jobs;
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