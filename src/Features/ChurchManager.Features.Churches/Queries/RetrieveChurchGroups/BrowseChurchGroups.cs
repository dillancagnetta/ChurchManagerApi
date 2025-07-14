using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Churches.Specifications;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Churches.Queries.RetrieveChurchGroups;

public record BrowseChurchGroups(string SearchTerm = null, bool IncludeDetails = true) : IRequest<ApiResponse>;

public class BrowseChurchGroupsQueryHandler(
    IGenericDbRepository<ChurchGroup> dbRepository,
    IPermissionContext permissions,
    IAppCurrentUser currentUser) : IRequestHandler<BrowseChurchGroups, ApiResponse>
{
    public async Task<ApiResponse> Handle(BrowseChurchGroups query, CancellationToken ct)
    {
        // This endpoint is public for now, so anyone can access it
        var allowedIds = currentUser?.Id == null // If user is not authenticated, return empty list
            ? null // all allowed
            : await permissions.GetAllowedIdsAsync<Church>(userLoginId:Guid.Parse(currentUser.Id), PermissionAction.View,   ct);
        
        var spec = new ChurchGroupsQuerySpecification(query.SearchTerm, query.IncludeDetails, allowedIds);

        var vm = await dbRepository.ListAsync<ChurchGroupViewModel>(spec, ct);

        return new ApiResponse(vm);
    }
}

/*
 * ------------------------------------------------
 */

public record ChurchesGroupsQuery(string SearchTerm = null, bool IncludeDetails = true) : IRequest<ApiResponse>;
public class ChurchesGroupsQueryHandler(IMediator mediator) : IRequestHandler<ChurchesGroupsQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(ChurchesGroupsQuery query, CancellationToken ct)
    {
        var apiResponse = await mediator.Send(new BrowseChurchGroups(query.SearchTerm, query.IncludeDetails), ct);

        return apiResponse;
    }
}
