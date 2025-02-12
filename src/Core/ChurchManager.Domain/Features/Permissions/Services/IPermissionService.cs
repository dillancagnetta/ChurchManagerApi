using Codeboss.Types;

namespace ChurchManager.Domain.Features.Permissions.Services;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync<T>(Guid userLoginId, int entityId, string permission, CancellationToken ct = default) where T : class, IEntity<int>;
    
    Task<IQueryable<T>> FilterByPermissionAsync<T>(Guid userLoginId, IQueryable<T> query, string permission, CancellationToken ct = default) where T : class, IEntity<int>;
    
    Task GrantPermissionAsync(int userLoginRoleId, string entityType, int[] entityIds, IEnumerable<string> permissions, CancellationToken ct = default);

    Task RevokePermissionAsync(int userLoginRoleId, string entityType, CancellationToken ct = default);
    
    Task<List<int>> GetAllowedEntityIdsAsync(Guid userLoginId, string entityType, string permission, CancellationToken ct = default);

}   