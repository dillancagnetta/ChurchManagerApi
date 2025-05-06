using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Churches.Commands.ChurchGroups;
public record DeleteChurchGroupCommand(int ChurchGroupId) : IRequest<ApiResponse>;

public class DeleteChurchGroupCommandHandler(
    IChurchGroupService service,
    IPersonDbRepository personDb) : IRequestHandler<DeleteChurchGroupCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(DeleteChurchGroupCommand command, CancellationToken ct)
    {
        await service.DeleteAsync(command.ChurchGroupId, ct);

        return new ApiResponse();
    }
}