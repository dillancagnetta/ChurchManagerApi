using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.Abstractions.Services;

public interface IChurchService
{
    Task<IReadOnlyList<ChurchViewModel>> ChurchListAsync(
        string searchTerm,
        CancellationToken ct = default);
}