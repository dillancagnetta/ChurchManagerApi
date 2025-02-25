using ChurchManager.Application.Abstractions.Services;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.UserLogins.Queries;

public record UserLoginRolesQuery(string SearchTerm = null, IEnumerable<int> ExcludeIds = null, Guid? UserLoginId = null): IRequest<ApiResponse>;

public class UserLoginRolesQueryHandler(ISecurityService service) : IRequestHandler<UserLoginRolesQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(UserLoginRolesQuery query, CancellationToken cancellationToken)
    {
        var results = await service.UserLoginRolesAsync(query.SearchTerm, query.ExcludeIds, query.UserLoginId, cancellationToken);
       
        return new ApiResponse(results);
    }
}