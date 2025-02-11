using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Communication.Commands;

public record CreateCommunicationCommand : IRequest<ApiResponse>
{
    public int[] PersonIds { get; set; }
    public string CommunicationType { get; set; } 
    public string Subject { get; set; } 
    public string Content { get; set; } 
    public int? CommunicationTemplateId { get; set; } 
    public int? SenderPersonId { get; set; }
    public DateTime? SendDateTime { get; set; }
    public int? ListGroupId { get; private set; }
}

public class CreateCommunicationHandler(ICommunicationDbRepository dbRepository) : IRequestHandler<CreateCommunicationCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreateCommunicationCommand command, CancellationToken ct)
    {
        var communication = new Domain.Features.Communications.Communication
        {
            CommunicationType = command.CommunicationType,
            Subject = command.Subject,
            SenderPersonId = command.SenderPersonId,
            FutureSendDateTime = command.SendDateTime,
            ListGroupId = command.ListGroupId,
            CommunicationTemplateId = command.CommunicationTemplateId,
            CommunicationContent = command.Content,
            Recipients = command.PersonIds.Select(id => new CommunicationRecipient { PersonId = id }).ToList(),
        };

        await dbRepository.AddAsync(communication, ct);
        
        return new ApiResponse("Communication created successfully.");
    }
}

/*
 * ----------------------------------------
 */

public record ApproveCommunicationCommand(int CommunicationId, int ApproverPersonId, string Note) : IRequest<ApiResponse>;

public class ApproveCommunicationHandler(ICommunicationDbRepository dbRepository)  : IRequestHandler<ApproveCommunicationCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ApproveCommunicationCommand command, CancellationToken ct)
    {
        var communication = await dbRepository.GetByIdAsync(command.CommunicationId, ct);

        if (communication is not null)
        {
               
            communication.Approve(new CommunicationReview
            {
                ReviewedDateTime = DateTime.UtcNow,
                ReviewerPersonId = command.ApproverPersonId,
                ReviewerNote = command.Note
            });
        
            await dbRepository.UpdateAsync(communication, ct);
            
            return new ApiResponse("Communication created successfully.");
        }
        
        return new ApiResponse("Communication not found.");
    }
}