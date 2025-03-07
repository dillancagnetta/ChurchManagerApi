using Codeboss.Types;

namespace ChurchManager.Domain.Features.Security.Services;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync<T>(Guid userLoginId, int entityId, PermissionAction permission, CancellationToken ct = default) where T : class, IEntity<int>;
    
    Task<IQueryable<T>> FilterByPermissionAsync<T>(Guid userLoginId, IQueryable<T> query, PermissionAction permission, CancellationToken ct = default) where T : class, IEntity<int>;
    
    Task GrantPermissionAsync(int userLoginRoleId, string entityType, int[] entityIds, IEnumerable<PermissionAction> permissions, CancellationToken ct = default);

    Task RevokePermissionAsync(int userLoginRoleId, string entityType, CancellationToken ct = default);
    
    Task<IReadOnlyList<int>> GetAllowedEntityIdsAsync<T>(Guid userLoginId, PermissionAction permission, CancellationToken ct = default) where T : class, IEntity<int>;

    Task<bool> IsSystemAdminAsync(Guid userLoginId, CancellationToken ct = default);
}   