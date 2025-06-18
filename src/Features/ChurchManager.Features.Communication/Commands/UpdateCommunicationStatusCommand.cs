using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Communication.Commands;

public record UpdateCommunicationStatusCommand(int CommunicationId, string Status,  string? Note) : IRequest<ApiResponse>;

public class UpdateCommunicationStatusHandler(ICommunicationDbRepository dbRepository, IAppCurrentUser currentUser)  : IRequestHandler<UpdateCommunicationStatusCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(UpdateCommunicationStatusCommand command, CancellationToken ct)
    {
        var communication = await dbRepository.GetByIdAsync(command.CommunicationId, ct);

        if (communication is not null)
        {
            // If its approved, raise domain event
            if (command.Status == CommunicationStatus.Approved.Value)
            {
                communication.Approve(new CommunicationReview
                {
                    ReviewedDateTime = DateTime.UtcNow,
                    ReviewerPersonId = currentUser.PersonId,
                    ReviewerNote = command.Note
                });
            }
            else
            {
                communication.UpdateStatus(command.Status, command.Note, currentUser.PersonId);
            }
            
            await dbRepository.UpdateAsync(communication, ct);
            
            return new ApiResponse("Communication status updated.");
        }
        
        return new ApiResponse("Communication not found.");
    }
}