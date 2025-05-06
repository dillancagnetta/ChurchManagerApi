using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.Features;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Events.Specifications;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.MultiTenant;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Events.Services;

public class EventTypeService(
    IPermissionContext permissions,
    ITenantCurrentUser currentUser,
    IGenericDbRepository<EventType> repository,
    IMapper mapper
    ) : CrudServiceAsync<EventType, EventTypeViewModel, EditEventTypeModel>(repository, mapper), IEventTypeService
{
    public async Task<IReadOnlyList<EventTypeViewModel>> AllEventsAsync(int? eventTypeId = null, bool? includeDetails = null, CancellationToken ct = default)
    {
        var allowedIds = await permissions.GetAllowedIdsAsync<EventType>(Guid.Parse(currentUser.Id), PermissionAction.View.Value, ct);
        
        var spec = new EventTypesListSpecification(allowedIds, includeDetails); 
        
        var vm = await Repository.ListAsync(spec, ct);
    
        return vm;
    }
}