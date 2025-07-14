using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Finances.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Features.Finances.Services;

public class FundsService(IFundsDbRepository dbRepository, IQueryCache cache) : IFundsService
{
    public Task<IEnumerable<FundViewModel>> FundsWithChildren(CancellationToken ct = default)
    {
        return cache.GetOrSetAsync("funds", () => dbRepository.FundsWithChildrenFlatAsync(ct), ct: ct);
    }
}