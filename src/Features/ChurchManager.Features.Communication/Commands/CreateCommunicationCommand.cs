using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using MediatR;

namespace ChurchManager.Features.Communication.Commands;

public record CreateCommunicationCommand : IRequest<ApiResponse>
{
    public int[] PersonIds { get; set; }
    public string CommunicationType { get; set; } 
    public string Name { get; set; } 
    public string? Category { get; set; } 
    public string? Subject { get; set; }
    public string Status { get; set; } = CommunicationStatus.PendingApproval.Value;
    public string Content { get; set; } 
    public int? CommunicationTemplateId { get; set; } 
    public DateTime? SendDateTime { get; set; }
    public int? ListGroupId { get;  set; }
    public bool IsBulkCommunication { get;  set; } = false;
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
            Status = command.Status,
            ListGroupId = command.ListGroupId,  
            CommunicationTemplateId = command.CommunicationTemplateId,
            //CommunicationContent = command.Content,
            Recipients = command.PersonIds.Select(id => new CommunicationRecipient { PersonId = id }).ToList(),
            IsBulkCommunication = command.IsBulkCommunication
        };

        if (!command.Content.IsNullOrEmpty())
        {
            communication.FormatTemplatedContent(command.Content);
        }
        
        await dbRepository.AddAsync(communication, ct);
        
        return new ApiResponse("Communication created.");
    }
}

/*
 * ----------------------------------------
 */

