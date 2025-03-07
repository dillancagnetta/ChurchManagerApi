using ChurchManager.Application.Abstractions.Services;
using ChurchManager.SharedKernel.Wrappers;
using Codeboss.Results;
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

/*
 * -----------------------------------
 */
 
public record ToggleUserLoginStatusCommand(Guid UserLoginId): IRequest<ApiResponse>;

public class ToggleUserLoginStatusHandler(ISecurityService service) : IRequestHandler<ToggleUserLoginStatusCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ToggleUserLoginStatusCommand command, CancellationToken cancellationToken)
    {
        OperationResult op = await service.ToggleUserLoginStatusAsync(command.UserLoginId, cancellationToken);
       
        return ApiResponse.FromOperation(op);
    }
}