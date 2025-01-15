#region

using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.History;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.Extensions;
using Codeboss.Types;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class HistoryDbRepository : GenericRepositoryBase<History>, IHistoryDbRepository
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public HistoryDbRepository(ChurchManagerDbContext dbContext, IDateTimeProvider dateTimeProvider) : base(dbContext)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task SaveChangesAsync(Type modelType, string category, int entityId, HistoryChangeList changes, string caption,
        Type relatedModelType, int? relatedEntityId, bool commitSave = true, int? modifiedByPersonId = null,
        CancellationToken cts = default)
    {
        if (changes.Any())
        {
            AddChanges(modelType, category, entityId, changes, caption, relatedModelType, relatedEntityId, modifiedByPersonId);
            if (commitSave)
            {
               await DbContext.SaveChangesAsync(cts);
            }
        }
    }

    public async Task AddChanges(Type modelType, string category, int entityId, HistoryChangeList changes, string caption,
        Type relatedModelType, int? relatedEntityId, int? modifiedByPersonId = null, CancellationToken cts = default)
    {
        List<History> historyRecordsToInsert = GetChanges(modelType, category, entityId, changes, caption,
            relatedModelType, relatedEntityId, modifiedByPersonId);

        await DbContext.AddRangeAsync(historyRecordsToInsert, cts);
    }

    internal List<History> GetChanges(Type modelType, string category, int entityId, HistoryChangeList changes,
        string caption, Type relatedModelType, int? relatedEntityId, int? modifiedByPersonId)
    {
        var entityType = modelType.Name;
        var creationDate = _dateTimeProvider.Now;

        int? relatedEntityTypeId = null;
        if (relatedModelType != null)
        {
            var relatedEntityType = relatedModelType.Name;
            if (relatedEntityId.HasValue)
            {
                relatedEntityTypeId = relatedEntityId.Value;
            }
        }

        List<History> historyRecordsToInsert = new List<History>(changes.Count);

        if (!entityType.IsNullOrEmpty() && !category.IsNullOrEmpty())
        {
            foreach (var historyChange in changes.Where(m => m != null))
            {
                var history = new History();
                history.EntityType = entityType;
                history.EntityId = entityId;
                history.Category = category;
                history.Caption = caption.Truncate(200);
                history.RelatedEntityId = relatedEntityId;

                historyChange.CopyToHistory(history);

                if (modifiedByPersonId.HasValue)
                {
                    // history.CreatedByPersonId = modifiedByPersonAliasId;
                }

                // If not specified, manually set the creation date on these history items so that they will be grouped together.
                if (historyChange.ChangedDateTime == null)
                {
                    history.CreatedDate = creationDate;
                }

                historyRecordsToInsert.Add(history);
            }
        }

        return historyRecordsToInsert;
    }
}