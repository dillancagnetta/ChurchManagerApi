using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Finances.Repositories;

public interface IFundsDbRepository
{
    Task<IEnumerable<FundViewModel>> FundsWithChildrenFlatAsync(CancellationToken ct = default);
}