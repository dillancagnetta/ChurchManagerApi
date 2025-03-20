using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Events.Specifications;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Events.Queries.Browse;

public record BrowseEventsQuery: QueryParameter,  IRequest<PagedResponse<EventViewModel>>
{
    public int? EventTypeId { get; set; }
    public int? ChurchGroupId { get; set; }
    public int? ChurchId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public bool? IsOnline { get; set; }
    public bool? IncludeDetails { get; set; }
}

public class BrowseEventsQueryHandler(
    IReadDbRepository<Event> readDb,
    IPermissionContext permissions,
    ICognitoCurrentUser currentUser) : IRequestHandler<BrowseEventsQuery, PagedResponse<EventViewModel>>
{
    public async Task<PagedResponse<EventViewModel>> Handle(BrowseEventsQuery query, CancellationToken ct)
    {
        var allowedIds = await permissions.GetAllowedIdsAsync<Event>(
            userLoginId:Guid.Parse(currentUser.Id), PermissionAction.View,   ct);
        
        var spec = new EventsListSpecification(allowedIds, query.EventTypeId, 
            query.ChurchGroupId, query.ChurchId, query.From, query.To, query.IsOnline, 
            query.IncludeDetails);

        var vm = await readDb.BrowseAsync<EventViewModel>(query, spec, ct);
        
        return new PagedResponse<EventViewModel>(vm);
    }
}