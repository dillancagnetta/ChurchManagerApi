using Ardalis.Specification;
using ChurchManager.Domain.Features.ChangeRequests.Extensions;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.ChangeRequests.Specifications;

public class ChangeRequestListSpecification : Specification<ChangeRequest, ChangeRequestViewModel>
{
    public ChangeRequestListSpecification(int? churchId,  IList<int>? personIds = null, string? status = null)
    {
        Query.AsNoTracking();
        
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
        
        Query.Select(x => x.ToViewModel(true));
    }
}