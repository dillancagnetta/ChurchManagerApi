#region

using ChurchManager.Domain.Features.History;
using ChurchManager.Domain.Features.People;
using EntityFrameworkCore.Triggered;
using Microsoft.Extensions.Logging;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Triggers;

public class FollowUpTrigger: IAfterSaveTrigger<FollowUp>
{
    private HistoryChangeList HistoryChanges { get; set; }

    private readonly IHistoryDbRepository _dbRepository;
    private readonly ILogger<FollowUpTrigger> _logger;

    public FollowUpTrigger(IHistoryDbRepository dbRepository, ILogger<FollowUpTrigger> logger)
    {
        _dbRepository = dbRepository;
        _logger = logger;
    }
    
    public Task AfterSave(ITriggerContext<FollowUp> context, CancellationToken cts)
    {
        HistoryChanges = new HistoryChangeList();

        if (context.ChangeType == ChangeType.Added)
        {
            var entity = context.Entity;
        }
        
        return Task.CompletedTask;
    }
}