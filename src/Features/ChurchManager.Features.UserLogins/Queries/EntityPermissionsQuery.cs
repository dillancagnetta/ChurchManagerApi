using ChurchManager.Application.Abstractions.Services;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.UserLogins.Queries;

public record EntityPermissionsQuery(IEnumerable<int> ExcludeIds = null, int? UserLoginRoleId = null): IRequest<ApiResponse>;

public class EntityPermissionsQueryHandler(ISecurityService service) : IRequestHandler<EntityPermissionsQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(EntityPermissionsQuery query, CancellationToken cancellationToken)
    {
        var results = await service.EntityPermissionsAsync(query.ExcludeIds, query.UserLoginRoleId, cancellationToken);
       
        return new ApiResponse(results);
    }
}