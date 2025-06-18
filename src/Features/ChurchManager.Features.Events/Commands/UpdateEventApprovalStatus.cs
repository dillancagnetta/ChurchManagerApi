using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Events.Commands;

public record UpdateEventApprovalStatus(int Id, string ApprovalStatus) : IRequest<ApiResponse>;

public class UpdateEventApprovalStatusHandler(
    IGenericDbRepository<Event> dbRepository) : IRequestHandler<UpdateEventApprovalStatus, ApiResponse>
{
    public async Task<ApiResponse> Handle(UpdateEventApprovalStatus command, CancellationToken ct)
    {
        var eventEntity  = await dbRepository.Queryable()
            .SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        
        if (eventEntity == null) return new ApiResponse("Event not found");

        if (eventEntity.ApprovalStatus.Value != command.ApprovalStatus)
        {
            eventEntity.ApprovalStatus = new ApprovalStatus(command.ApprovalStatus);
        
            await dbRepository.SaveChangesAsync(ct); 
        }
        
        return new ApiResponse();
    }
}