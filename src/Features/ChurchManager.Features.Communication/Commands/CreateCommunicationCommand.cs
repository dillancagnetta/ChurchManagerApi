using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Communication.Commands;

public record CreateCommunicationCommand : IRequest<ApiResponse>
{
    public int[] PersonIds { get; set; }
    public string CommunicationType { get; set; } 
    public string Name { get; set; } 
    public string Category { get; set; } 
    public string Subject { get; set; } 
    public string Content { get; set; } 
    public int? CommunicationTemplateId { get; set; } 
    public DateTime? SendDateTime { get; set; }
    public int? ListGroupId { get;  set; }
    public bool IsBulkCommunication { get;  set; }
}

public class CreateCommunicationHandler(ICommunicationDbRepository dbRepository, IAppCurrentUser currentUser) : IRequestHandler<CreateCommunicationCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreateCommunicationCommand command, CancellationToken ct)
    {
        var communication = new Domain.Features.Communications.Communication
        {
            Name = command.Name,
            CommunicationType = command.CommunicationType,
            Subject = command.Subject,
            SenderPersonId = currentUser.PersonId,
            FutureSendDateTime = command.SendDateTime,
            Category = command.Category,
            ListGroupId = command.ListGroupId,  
            CommunicationTemplateId = command.CommunicationTemplateId,
            CommunicationContent = command.Content,
            Recipients = command.PersonIds.Select(id => new CommunicationRecipient { PersonId = id }).ToList(),
            IsBulkCommunication = command.IsBulkCommunication
        };

        await dbRepository.AddAsync(communication, ct);
        
        return new ApiResponse("Communication created successfully.");
    }
}

/*
 * ----------------------------------------
 */

public record ApproveCommunicationCommand(int CommunicationId,  string Note) : IRequest<ApiResponse>;

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