using ChurchManager.Application.Abstractions.Services;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.UserLogins.Queries;

public record UserLoginsQuery(string SearchTerm): IRequest<ApiResponse>;

public class UserLoginsQueryHandler(ISecurityService service) : IRequestHandler<UserLoginsQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(UserLoginsQuery request, CancellationToken cancellationToken)
    {
        var results = await service.UserLoginsAsync(request.SearchTerm, cancellationToken);
       
        return new ApiResponse(results);
    }
}