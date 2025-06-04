using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Communications.Extensions;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.Communications.Specifications;

public class CommunicationPreferencesTypesSpecification : Specification<CommunicationPreferenceType>
{
    public CommunicationPreferencesTypesSpecification(bool canOverride = true, bool includeDetails = false)
    {
        Query.AsNoTracking();
        
        Query.EnableCache(nameof(CommunicationPreferencesTypesSpecification),
            CacheKeyExtensions.GenerateCacheKey(canOverride, includeDetails));
        
        if (includeDetails)
        {
            Query.Include(x => x.Preferences);
        }
        
        Query.Where(g => g.CanOverride == canOverride);

        //Query.Select(x => x.ToViewModel());
    }
}