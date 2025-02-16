using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Churches.Specifications;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.MultiTenant;

namespace ChurchManager.Features.Churches.Services;

public class ChurchService(
    IPermissionContext permissions,
    ITenantCurrentUser currentUser,
    IGenericDbRepository<Church> dbRepository) : IChurchService
{
    public async Task<IReadOnlyList<ChurchViewModel>> ChurchListAsync(string searchTerm, CancellationToken ct = default)
    {
        var allowedIds = await permissions.GetAllowedIdsAsync<Church>(Guid.Parse(currentUser.Id), PermissionAction.View.Value, ct);
        
        var spec = new ChurchesListSpecification(allowedIds, searchTerm);
        
        var vm = await dbRepository.ListAsync(spec, ct);
    
        return vm;
    }
}