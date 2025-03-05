using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.Abstractions.Services;

public interface IChurchService : ICrudServiceAsync<Church, ChurchViewModel, EditChurchModel>
{
    Task<IReadOnlyList<ChurchViewModel>> ChurchListAsync(
        string searchTerm,
        CancellationToken ct = default);
}