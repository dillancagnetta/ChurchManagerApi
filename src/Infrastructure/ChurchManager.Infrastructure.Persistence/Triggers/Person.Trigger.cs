#region

using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.History;
using ChurchManager.Domain.Features.People;
using CodeBoss.Extensions;
using EntityFrameworkCore.Triggered;
using Microsoft.Extensions.Logging;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Triggers;

public class PersonTrigger : IAfterSaveTrigger<Person> 
{
    private HistoryChangeList HistoryChanges { get; set; }

    private readonly IHistoryDbRepository _dbRepository;
    private readonly ILogger<PersonTrigger> _logger;

    public PersonTrigger(IHistoryDbRepository dbRepository, ILogger<PersonTrigger> logger)
    {
        _dbRepository = dbRepository;
        _logger = logger;
    }
    
    public async Task AfterSave(ITriggerContext<Person> context, CancellationToken cts)
    {
        HistoryChanges = new HistoryChangeList();

        if (context.ChangeType == ChangeType.Added)
        { 
           var person = context.Entity;
           var fullName = person.FullName.ToString();
           
           HistoryChanges.AddChange( HistoryVerb.Add, HistoryChangeType.Record, "Person" ).SetNewValue( fullName );
           History.EvaluateChange( HistoryChanges, "Connection Status", null, person.ConnectionStatus);
           
           // Check if we updated Baptism Info
           if (person.BaptismStatus?.IsBaptised.HasValue ?? false)
           {
               History.EvaluateChange( HistoryChanges, "Baptism Status", null, person.BaptismStatus.IsBaptised.Value);

               if (person.BaptismStatus.BaptismDate.HasValue)
               {
                   History.EvaluateChange( HistoryChanges, "Baptism Date", null, person.BaptismStatus.BaptismDate.Value);
               }
           }

           if (!person.PhotoUrl.IsNullOrEmpty())
           {
               HistoryChanges.AddChange( HistoryVerb.Add, HistoryChangeType.Property, "Photo" );
           }
           
           await _dbRepository.SaveChangesAsync(
                typeof(Person),
                "Add New Person",
                person.Id,
                HistoryChanges,
                caption:person.FullName.ToString(),
                relatedModelType:null,
                relatedEntityId:null,
                commitSave:true,
                modifiedByPersonId:null,
                cts);
            
            // await _dbContext.SaveChangesAsync(cts);
            _logger.LogInformation($"[{nameof(PersonTrigger)}] completed with [{HistoryChanges.Count}] changes.");
        }
    }
}