using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.ChangeRequests;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Common.Commands;

public record ApproveChangeRequestCommand(int ChangeRequestId, string? Note = null): IRequest<ApiResponse>;

public class ApproveChangeHandler(IGenericDbRepository<ChangeRequest> dbRepository, IAppCurrentUser currentUser) : IRequestHandler<ApproveChangeRequestCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ApproveChangeRequestCommand command, CancellationToken ct)
    {
        var changeRequest = await dbRepository.GetByIdAsync(command.ChangeRequestId, ct);
        
        if (changeRequest is not null && changeRequest.Status != ApprovalStatus.Approved.Value)
        {
            changeRequest.Approve(currentUser.PersonId, command.Note);
            
            await dbRepository.UpdateAsync(changeRequest, ct);
            
            return new ApiResponse("Change Request approved successfully.");
        }
        
        return new ApiResponse("Change Request not found.");
    }
}