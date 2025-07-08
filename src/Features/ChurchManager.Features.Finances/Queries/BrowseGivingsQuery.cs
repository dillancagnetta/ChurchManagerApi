using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Specifications;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.MultiTenant;
using MediatR;

namespace ChurchManager.Features.Finances.Queries;

public record BrowseGivingsQuery : SearchTermQueryParameter, IRequest<PagedResponse<GivingViewModel>>
{
    public string[]? GivingTypes { get; set; } = []; // First Fruit, Tithe, Partnership
    public string[]? PaymentMethods { get; set; } = []; // EFT, Unknown, Cash
    public string[]? BenefactorTypes { get; set; } = []; // EFT, Unknown, Cash
    public AutocompleteResult? Person { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Currency { get; set; } = "ZAR";
    public int? FundId { get; set; }
}

public class BrowseGivingsHandler(IReadDbRepository<Giving> givingsDb, IAppCurrentUser currentUser) : IRequestHandler<BrowseGivingsQuery, PagedResponse<GivingViewModel>>
{
    public async Task<PagedResponse<GivingViewModel>> Handle(BrowseGivingsQuery query, CancellationToken ct)
    {
        var test = currentUser;
        var spec = new BrowseGivingsSpecification(
            query,
            query.Person?.Id,
            query.SearchTerm,
            query.GivingTypes, query.PaymentMethods, query.Currency,
            query.FundId,
            query.BenefactorTypes,
            query.From, query.To);
        
        var pagedResult = await givingsDb.BrowseAsync(query, spec, ct);

        return new PagedResponse<GivingViewModel>(pagedResult);
    }
}

