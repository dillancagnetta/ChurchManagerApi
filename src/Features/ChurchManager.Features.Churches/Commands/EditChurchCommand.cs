using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.Features.Churches.Infrastructure.Mapper;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Churches.Commands;

public record EditChurchCommand(int Id, string Name, string Description, int? LeaderPersonId, int ChurchGroupId)
    : IRequest<ApiResponse>
{
    public ICollection<ChurchServiceTimeViewModel> ServiceTimes { get; set; } = [];
    public string? ShortCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}

public class EditChurchCommandHandler(
    IChurchService service) : IRequestHandler<EditChurchCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(EditChurchCommand command, CancellationToken ct)
    {
        var operationResult =  await service.UpdateChurchAsync(command.ToModel(), ct);

        if (!operationResult.IsSuccess) return new ApiResponse(operationResult.Errors.First().Message);

        var church = operationResult.Result;
        
        // Needed because we rerender the list - so we need this data
        return new ApiResponse(church);
    }
}