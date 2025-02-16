using Codeboss.Types;

namespace ChurchManager.Domain.Features.Security.Services;

public interface IPermissionContext
{
    Task<IEnumerable<int>> GetAllowedIdsAsync<T>(
        Guid userLoginId, 
        PermissionAction permission, 
        CancellationToken ct = default) where T : class, IEntity<int>;
}