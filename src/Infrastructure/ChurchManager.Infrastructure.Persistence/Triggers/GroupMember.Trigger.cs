using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.History;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using CodeBoss.Extensions;
using EntityFrameworkCore.Triggered;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Persistence.Triggers;

public class GroupMemberTrigger : IAfterSaveTrigger<GroupMember>
{
    private HistoryChangeList HistoryChanges { get; set; }

    private readonly IHistoryDbRepository _dbRepository;
    private readonly IPersonDbRepository _personDbRepository;
    private readonly ILogger<GroupMemberTrigger> _logger;

    public GroupMemberTrigger(
        IHistoryDbRepository dbRepository,
        IPersonDbRepository personDbRepository,
        ILogger<GroupMemberTrigger> logger)
    {
        _dbRepository = dbRepository;
        _personDbRepository = personDbRepository;
        _logger = logger;
    }
    public async Task AfterSave(ITriggerContext<GroupMember> context, CancellationToken cts)
    {
        HistoryChanges = new HistoryChangeList();
        
        var entity = context.Entity;
        var groupMemberString = nameof(GroupMember).SplitCase();
        var groupRoleString = nameof(GroupMember.GroupRole).SplitCase();
        
        var person = await _personDbRepository.GetByIdAsync(entity.PersonId);
        var fullName = person!.FullName.ToString();
        
        string category = string.Empty;
        string caption = string.Empty;
        
        /* Added */
        if (context.ChangeType == ChangeType.Added)
        {
            HistoryChanges.AddChange( HistoryVerb.Add, HistoryChangeType.Record, groupMemberString).SetNewValue( fullName );
            History.EvaluateChange( HistoryChanges, groupRoleString, null, entity.GroupRole.Name);

            category = $"Add {groupMemberString}";
            caption = $"{person.FullName} added to Group";

        }
        
        /* Specific Properties */
        if (entity.FirstVisitDate != null)
        {
            History.EvaluateChange( HistoryChanges, nameof(entity.FirstVisitDate), null, entity.FirstVisitDate);
        }
        
        /* Modified */
        if (context.ChangeType == ChangeType.Modified)
        {
            category = $"Updated {groupMemberString}";
            caption = $"{person.FullName} updated in Group";
            
            if (context.UnmodifiedEntity?.GroupRoleId != entity.GroupRoleId)
            {
                HistoryChanges.AddChange( HistoryVerb.Modify, HistoryChangeType.Property, "Group Role").SetNewValue( entity.GroupRoleId.ToString() );
            }
        }
        
        await _dbRepository.SaveChangesAsync(
            typeof(GroupMember),
            category,
            entity.Id,
            HistoryChanges,
            caption,
            relatedModelType:typeof(Person),
            relatedEntityId:entity.PersonId,
            commitSave:true,
            modifiedByPersonId:null,
            cts);
        
        // await _dbContext.SaveChangesAsync(cts);
        _logger.LogInformation($"[{nameof(GroupMemberTrigger)}] completed with [{HistoryChanges.Count}] changes.");
    }
}