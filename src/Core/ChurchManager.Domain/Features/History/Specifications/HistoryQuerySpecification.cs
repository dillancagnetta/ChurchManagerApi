using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.History.Specifications;

public class HistoryQuerySpecification : Specification<History>
{
    public HistoryQuerySpecification(IPagedQuery paging, string entityType, int entityId)
    {
        Query.Where(x => x.EntityType == entityType);
        Query.Where(x => x.EntityId == entityId);

        Query.OrderBy(x => x.CreatedDate);
        
        Query
            .Skip(paging.CalculateSkip())
            .Take(paging.CalculateTake());
    }
}