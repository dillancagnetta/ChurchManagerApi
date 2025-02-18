using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.UserLogins.Queries;

public record BrowsePermissionsQuery : SearchTermQueryParameter,  IRequest<PagedResponse<PermissionViewModel>>
{
    public string EntityType { get; set; }
    public string ScopeType { get; set; }
    public int? EntityId { get; set; }
    public bool? IsDynamicScope { get; set; }
}

public class PermissionsQueryHandler(ISecurityService service) : IRequestHandler<BrowsePermissionsQuery, PagedResponse<PermissionViewModel>>
{
    public Task<PagedResponse<PermissionViewModel>> Handle(BrowsePermissionsQuery request, CancellationToken ct)
    {
        var result = service.BrowsePermissionsAsync(request, request.SearchTerm, request.EntityId, request.IsDynamicScope, ct);
        
        return result;
    }
}