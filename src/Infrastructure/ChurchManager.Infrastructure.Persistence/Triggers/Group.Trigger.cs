#region

using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.History;
using EntityFrameworkCore.Triggered;
using Microsoft.Extensions.Logging;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Triggers;

public class GroupTrigger : IAfterSaveTrigger<Group>
{
    private HistoryChangeList HistoryChanges { get; set; }

    private readonly IHistoryDbRepository _dbRepository;
    private readonly ILogger<GroupTrigger> _logger;

    public GroupTrigger(IHistoryDbRepository dbRepository, ILogger<GroupTrigger> logger)
    {
        _dbRepository = dbRepository;
        _logger = logger;
    }
    public Task AfterSave(ITriggerContext<Group> context, CancellationToken cts)
    {
        HistoryChanges = new HistoryChangeList();

        if (context.ChangeType == ChangeType.Added)
        {
            var entity = context.Entity;
        }
        
        return Task.CompletedTask;
    }
}