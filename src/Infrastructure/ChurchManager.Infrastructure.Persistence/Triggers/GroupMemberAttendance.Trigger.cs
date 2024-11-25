using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.History;
using EntityFrameworkCore.Triggered;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Persistence.Triggers;

public class GroupMemberAttendanceTrigger : IAfterSaveTrigger<GroupMemberAttendance>
{
    private HistoryChangeList HistoryChanges { get; set; }

    private readonly IHistoryDbRepository _dbRepository;
    private readonly ILogger<GroupMemberAttendanceTrigger> _logger;

    public GroupMemberAttendanceTrigger(IHistoryDbRepository dbRepository, ILogger<GroupMemberAttendanceTrigger> logger)
    {
        _dbRepository = dbRepository;
        _logger = logger;
    }
    public Task AfterSave(ITriggerContext<GroupMemberAttendance> context, CancellationToken cts)
    {
        HistoryChanges = new HistoryChangeList();

        if (context.ChangeType == ChangeType.Added)
        {
            var entity = context.Entity;
        }
        
        return Task.CompletedTask;
    }
}