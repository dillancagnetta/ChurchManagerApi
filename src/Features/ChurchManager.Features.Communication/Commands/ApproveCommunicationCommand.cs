using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Communication.Commands;
public record ApproveCommunicationCommand(int CommunicationId,  string? Note) : IRequest<ApiResponse>;

public class ApproveCommunicationHandler(ICommunicationDbRepository dbRepository, IAppCurrentUser currentUser)  : IRequestHandler<ApproveCommunicationCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ApproveCommunicationCommand command, CancellationToken ct)
    {
        var communication = await dbRepository.GetByIdAsync(command.CommunicationId, ct);

        if (communication is not null && communication.Status != CommunicationStatus.Approved.Value)
        {
            communication.Approve(new CommunicationReview
            {
                ReviewedDateTime = DateTime.UtcNow,
                ReviewerPersonId = currentUser.PersonId,
                ReviewerNote = command.Note
            });
        
            await dbRepository.UpdateAsync(communication, ct);
            
            return new ApiResponse("Communication approved successfully.");
        }
        
        return new ApiResponse("Communication not found.");
    }
}