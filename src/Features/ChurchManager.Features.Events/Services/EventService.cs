using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.Features;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.MultiTenant;

namespace ChurchManager.Features.Events.Services;

public class EventService(
    IPermissionContext permissions,
    ITenantCurrentUser currentUser,
    IGenericDbRepository<Event> repository,
    IMapper mapper
) : CrudServiceAsync<Event, EventViewModel, EditEventViewModel>(repository, mapper), IEventService
{
    
}