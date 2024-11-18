using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.History;

public interface IHistoryDbRepository : IGenericDbRepository<History>
{
    Task SaveChangesAsync(
        Type modelType, string category, int entityId, HistoryChangeList changes, string caption, 
        Type relatedModelType, int? relatedEntityId, bool commitSave = true, int? modifiedByPersonId = null,
        CancellationToken cts = default);
    
    Task AddChanges(Type modelType, string category, int entityId, HistoryChangeList changes, string caption, 
        Type relatedModelType, int? relatedEntityId, int? modifiedByPersonId = null, CancellationToken cts = default);
}