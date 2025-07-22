using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Finances.Repositories;

public interface IFundsDbRepository
{
    Task<IEnumerable<FundViewModel>> FundsWithChildrenFlatAsync(bool onlyShowInNavigation = true, CancellationToken ct = default);
}