using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Churches.Commands;

public record DeleteChurchCommand(int ChurchGroupId) : IRequest<ApiResponse>;

public class DeleteChurchCommandHandler(
    IChurchService service,
    IPersonDbRepository personDb) : IRequestHandler<DeleteChurchCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(DeleteChurchCommand command, CancellationToken ct)
    {
        await service.DeleteAsync(command.ChurchGroupId, ct);

        return new ApiResponse();
    }
}