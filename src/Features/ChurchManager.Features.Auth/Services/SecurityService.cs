using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Features.Security.Specifications;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using Convey.CQRS.Queries;

namespace ChurchManager.Features.Auth.Services;

public class SecurityService(IPermissionContext permissions,
    IGenericDbRepository<UserLogin> userLoginDb,
    IGenericDbRepository<UserLoginRole> rolesDb,
    IGenericDbRepository<EntityPermission> permissionsDb,
    ICognitoCurrentUser currentUser
    ) : ISecurityService
{
    public async Task<IEnumerable<UserLoginViewModel>> UserLoginsAsync(string searchTerm, CancellationToken ct = default)
    {
        var spec = new UserLoginsSpecification(searchTerm);
        
        var vm = await userLoginDb.ListAsync(spec, ct);
        return vm;
    }

    public async Task<IEnumerable<UserLoginRoleViewModel>> UserLoginRolesAsync(string searchTerm, CancellationToken ct = default)
    {
        var spec = new UserLoginRolesSpecification(searchTerm);
        
        var vm = await rolesDb.ListAsync(spec, ct);
        
        return vm;
    }

    public async Task<PagedResponse<PermissionViewModel>> BrowsePermissionsAsync(IPagedQuery query, string searchTerm, int? entityId = null, bool? isDynamicScope = null,
        CancellationToken ct = default)
    {
        var spec = new BrowsePermissionSpecification(searchTerm, entityId, isDynamicScope);
        
        var pagedResult = await permissionsDb.BrowseAsync(query, spec, ct);
        
        return new PagedResponse<PermissionViewModel>(pagedResult);
    }
}