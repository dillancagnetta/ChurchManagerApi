﻿using ChurchManager.Domain.Features.Permissions;
using ChurchManager.Domain.Features.Permissions.Services;
using Codeboss.Types;

namespace ChurchManager.Features.Auth.Services;

public class PermissionContext(IPermissionService permissionService) : IPermissionContext
{
    public async Task<IEnumerable<int>> GetAllowedIdsAsync<T>(Guid userLoginId, PermissionAction permission, CancellationToken ct = default) where T : class, IEntity<int>
    {
        return await permissionService.GetAllowedEntityIdsAsync<T>(
            userLoginId,
            permission,
            ct);
    }
}