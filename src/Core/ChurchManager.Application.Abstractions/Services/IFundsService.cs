using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.Abstractions.Services;

public interface IFundsService
{
    Task<IEnumerable<FundViewModel>> FundsWithChildren(CancellationToken ct = default);
}