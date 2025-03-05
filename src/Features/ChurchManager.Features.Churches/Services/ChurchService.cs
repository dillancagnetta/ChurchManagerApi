using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.Features;
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
    IGenericDbRepository<Church> dbRepository,
    IMapper mapper) : CrudServiceAsync<Church, ChurchViewModel, EditChurchModel>(dbRepository, mapper), IChurchService
{
    public async Task<IReadOnlyList<ChurchViewModel>> ChurchListAsync(string searchTerm, CancellationToken ct = default)
    {
        var allowedIds = await permissions.GetAllowedIdsAsync<Church>(Guid.Parse(currentUser.Id), PermissionAction.View.Value, ct);
        
        var spec = new ChurchesListSpecification(allowedIds, searchTerm);
        
        var vm = await Repository.ListAsync(spec, ct);
    
        return vm;
    }
}