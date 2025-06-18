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
        IPagedQuery paging, int? churchId, int? personId = null, string? status = null,string? entityType = null, DateTime? from = null, DateTime? to = null)
    {
        Query.AsNoTracking();
        
        /*
        Query.EnableCache(nameof(BrowseChangeRequestSpecification),
            CacheKeyExtensions.GenerateCacheKey(paging, personId, churchId, status, entityType, from, to));
            */
        
        Query.Include(x => x.Properties);

        if (churchId.HasValue)
        {
            Query.Where(x => x.ChurchId == churchId.Value);
        }
        
        if (!status.IsNullOrEmpty())
        {
            Query.Where(x => x.Status == status);
        }
        
        if (!entityType.IsNullOrEmpty())
        {
            Query.Where(x => x.Properties.All(s => s.EntityType == entityType));
        }
        
        if (personId.HasValue)
        {
            Query.Where(x => x.Properties.All(s => 
                s.EntityType == "Person" && personId! == s.EntityId));
        }

        Query.OrderByDescending(x => x.RequestedDate);
        
        Query
            .Skip(paging.CalculateSkip())
            .Take(paging.CalculateTake());
        
        Query.Select(x => x.ToViewModel(true));
    }
}