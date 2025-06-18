using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Security.Services;

public interface IEntityPermissionsResolver
{
    Task<IReadOnlyList<SelectItemViewModel>> ResolveAsync(string entityType, int[] entityids, CancellationToken ct = default);
}