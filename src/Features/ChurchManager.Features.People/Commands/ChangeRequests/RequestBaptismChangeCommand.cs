using ChurchManager.Application.Abstractions.Services;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.People.Commands.ChangeRequests;

public record RequestBaptismChangeCommand : IRequest<ApiResponse>
{
    public int PersonId { get; set; }
    public bool? IsBaptised { get; set; }
    public DateTime? BaptismDate { get; set; }
    public string? Reason { get; set; }
}

public class RequestBaptismChangeHandler(IChangeRequestService service) : IRequestHandler<RequestBaptismChangeCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(RequestBaptismChangeCommand command, CancellationToken ct)
    {
        var operationResult = await service.CreateBaptismChangeRequestAsync(
            command.PersonId, command.IsBaptised, command.BaptismDate, command.Reason);
        
        return ApiResponse.FromOperation(operationResult);
    }
}

/*
 * -------------------------------------------------------------------------
 */
 
public record ReceivedHolySpiritChangeCommand : IRequest<ApiResponse>
{
    public int PersonId { get; set; }
    public bool ReceivedHolySpirit { get; set; }
    public string? Reason { get; set; }
}

public class ReceivedHolySpiritChangeHandler(IChangeRequestService service) : IRequestHandler<ReceivedHolySpiritChangeCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ReceivedHolySpiritChangeCommand command, CancellationToken ct)
    {
        var operationResult = await service.CreateHolySpiritChangeRequestAsync(
            command.PersonId, command.ReceivedHolySpirit, command.Reason);
        
        return ApiResponse.FromOperation(operationResult);
    }
}