using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.ChangeRequests.Extensions;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.ChangeRequests.Specifications;

public class BrowseChangeRequestSpecification : Specification<ChangeRequest, ChangeRequestViewModel>
{
    public BrowseChangeRequestSpecification(
        IPagedQuery paging, int? churchId,  IList<int>? personIds = null, string? status = null, DateTime? from = null, DateTime? to = null)
    {
        Query.AsNoTracking();
        
        Query.EnableCache(nameof(BrowseChangeRequestSpecification),
            CacheKeyExtensions.GenerateCacheKey(paging, personIds, churchId, status, from, to));
        
        Query.Include(x => x.Properties);

        if (churchId.HasValue)
        {
            Query.Where(x => x.ChurchId == churchId.Value);
        }
        
        if (!status.IsNullOrEmpty())
        {
            Query.Where(x => x.Status == status);
        }
        
        if (!personIds.IsNullOrEmpty())
        {
            Query.Where(x => x.Properties.All(s => 
                s.EntityType == "Person" && personIds!.Contains(s.EntityId)));
        }

        Query.OrderByDescending(x => x.RequestedDate);
        
        Query
            .Skip(paging.CalculateSkip())
            .Take(paging.CalculateTake());
        
        Query.Select(x => x.ToViewModel(true));
    }
}