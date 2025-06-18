using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Communications.Extensions;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.Communications.Specifications;

public class CommunicationPreferencesQuerySpecification : Specification<CommunicationPreference>
{
    public CommunicationPreferencesQuerySpecification(CommunicationType? communicationType, int? personId)
    {
        Query.AsNoTracking();
        
        /*Query.EnableCache(nameof(CommunicationPreferencesQuerySpecification),
            CacheKeyExtensions.GenerateCacheKey(name, communicationType, personId));*/
        
        /*if (!name.IsNullOrWhiteSpace())
        {
            Query.Where(cg =>
                EF.Functions.ILike(cg.Name, $"%{name}%"));
        }*/
        
        if (communicationType is not null)
        {
            //Query.Include(x => x.PreferenceType);
            Query.Where(p => p.CommunicationType == communicationType);
        }
        
        if (personId.HasValue)
        {
            //Query.Include(x => x.Preferences);
            Query.Where(p => p.PersonId == personId);
        }

        //Query.Select(x => x.ToViewModel());
    }
}