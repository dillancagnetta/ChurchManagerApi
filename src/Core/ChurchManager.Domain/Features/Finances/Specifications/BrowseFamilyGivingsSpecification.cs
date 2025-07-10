using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Finances.Extensions;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Finances.Specifications;

public class BrowseFamilyGivingsSpecification: Specification<Giving, GivingViewModel>
{
    public BrowseFamilyGivingsSpecification(
        IPagedQuery paging,
        int[] personIds,
        string? paymentReference,
        string[]? givingTypes,
        string[]? paymentMethods,
        string? currency,
        int? fundId,
        int? churchId,
        string[]? benefactorTypes,
        DateTime? from, DateTime? to
        )
    {
        Query.EnableCache(nameof(BrowseFamilyGivingsSpecification),
            CacheKeyExtensions.GenerateCacheKey(paging, personIds, paymentReference, givingTypes, paymentMethods, currency, fundId, churchId, benefactorTypes, from, to));
        Query.AsNoTracking();
        Query.Include(x => x.Benefactor);
        Query.Include(x => x.Fund);
        
        // Persons Filter
        if(!personIds.IsNullOrEmpty())
        {
            Query.Where(g => g.Benefactor!.PersonId.HasValue && personIds.Contains(g.Benefactor.PersonId.Value));
        }
        
        // Church Filter
        if(churchId.HasValue)
        {
            Query.Where(g => g.ChurchId == churchId);
        }
        
        // Payment Reference Filter
        if(!paymentReference.IsNullOrEmpty())
        {
            paymentReference = paymentReference!.ToUpperInvariant();
            Query.Where(g => g.ParsedReference != null && g.ParsedReference.Contains(paymentReference));
        }
        
        // Type Filter
        if (!givingTypes.IsNullOrEmpty())
        {
            var _givingTypes = givingTypes.ToList();
            Query.Where(g => _givingTypes.Contains(g.GivingType));
        }
        
        // Payment Methods Filter
        if (!paymentMethods.IsNullOrEmpty())
        {
            var _paymentMethods = paymentMethods.ToList();
            Query.Where(g => _paymentMethods.Contains(g.PaymentMethod));
        }
        
        // Fund Filter
        if(fundId.HasValue)
        {
            Query.Where(g => g.FundId == fundId);
        }
        
        // Currency Filter
        if(!currency.IsNullOrEmpty())
        {
            Query.Where(g => g.GivingAmount.Currency == currency);
        }
        
        // Benefactor Types Filter
        if(!benefactorTypes.IsNullOrEmpty())
        {
            var _benefactorTypes = benefactorTypes.ToList();
            Query.Where(g => _benefactorTypes.Contains(g.Benefactor!.Type));
            //Query.Where(g => benefactorTypes.Any(t => t == g.Benefactor!.Type));
        }
        
        // Date Filters
        if(from.HasValue)
        {
            Query.Where(g => g.Date >= from.Value);
        }
        if(to.HasValue)
        {
            Query.Where(g => g.Date <= to.Value);
        }
        
        Query.OrderByDescending(x => x.Date);

        Query
            .Skip(paging.CalculateSkip())
            .Take(paging.CalculateTake());
        
        Query.Select(x => x.ToModel());
    }
}