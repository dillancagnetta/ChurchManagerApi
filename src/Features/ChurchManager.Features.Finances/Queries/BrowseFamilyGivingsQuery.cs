using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Specifications;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Finances.Queries;

public record BrowseFamilyGivingsQuery : BrowseGivingsQuery
{
    public int FamilyId { get; set; }
};

public class BrowseFamilyGivingsQueryHandler(IReadDbRepository<Giving> givingsDb, IFamilyDbRepository familyDb) : IRequestHandler<BrowseFamilyGivingsQuery, PagedResponse<GivingViewModel>>
{
    public async Task<PagedResponse<GivingViewModel>> Handle(BrowseFamilyGivingsQuery query, CancellationToken ct)
    {
        var familyMembersPersonIds = await familyDb.PersonIdsOfFamilyMembersAsync(query.FamilyId, ct);
        
        var spec = new BrowseFamilyGivingsSpecification(
            query,
            familyMembersPersonIds.ToArray(),
            query.SearchTerm,
            query.GivingTypes, query.PaymentMethods, query.Currency,
            query.FundId,
            query.ChurchId,
            query.BenefactorTypes,
            query.From, query.To);
        
        var pagedResult = await givingsDb.BrowseAsync(query, spec, ct);

        return new PagedResponse<GivingViewModel>(pagedResult);
    }
}

