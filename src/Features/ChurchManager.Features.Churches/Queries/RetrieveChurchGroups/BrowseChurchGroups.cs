using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Churches.Specifications;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Churches.Queries.RetrieveChurchGroups;

public record BrowseChurchGroups(string SearchTerm = null, bool IncludeDetails = true) : IRequest<ApiResponse>;

public class BrowseChurchGroupsQueryHandler(
    IGenericDbRepository<ChurchGroup> dbRepository,
    IPermissionContext permissions,
    ICognitoCurrentUser currentUser) : IRequestHandler<BrowseChurchGroups, ApiResponse>
{
    public async Task<ApiResponse> Handle(BrowseChurchGroups query, CancellationToken ct)
    {
        var allowedIds = await permissions.GetAllowedIdsAsync<Church>(
            userLoginId:Guid.Parse(currentUser.Id), PermissionAction.View,   ct);
        
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

/*
 * ------------------------------------------------
 */

public record AddChurchGroupCommand(string Name, string Description, int? LeaderPersonId) : IRequest<ApiResponse>;
public class AddChurchGroupCommandHandler(
    IServiceAsync<ChurchGroup, ChurchGroupViewModel> service,
    IPersonDbRepository personDb) : IRequestHandler<AddChurchGroupCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(AddChurchGroupCommand command, CancellationToken ct)
    {
        var dto = new ChurchGroupViewModel
        {
            Name = command.Name,
            Description = command.Description,
            LeaderPerson = command.LeaderPersonId.HasValue
                // Needed because we rerender the list - so we need this data
                ? await personDb.BasicPersonViewModelAsync(command.LeaderPersonId.Value, ct)
                : null
        };
        
        await service.AddAsync(dto, ct);

        return new ApiResponse(dto);
    }
}