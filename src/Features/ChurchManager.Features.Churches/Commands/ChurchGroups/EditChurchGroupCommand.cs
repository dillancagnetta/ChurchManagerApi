using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Churches.Commands.ChurchGroups;
public record EditChurchGroupCommand(int Id, string Name, string Description, int? LeaderPersonId) : IRequest<ApiResponse>;
public class EditChurchGroupCommandHandler(
    IChurchGroupService service,
    IPersonDbRepository personDb) : IRequestHandler<EditChurchGroupCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(EditChurchGroupCommand command, CancellationToken ct)
    {
        var dto = new EditChurchGroupModel
        {
            Id = command.Id,
            Name = command.Name,
            Description = command.Description,
            LeaderPersonId = command.LeaderPersonId
        };
        
        var vm = await service.UpdateAsync(dto, ct);

        // Needed because we rerender the list - so we need this data
        vm.LeaderPerson = command.LeaderPersonId.HasValue
            ? await personDb.BasicPersonViewModelAsync(command.LeaderPersonId.Value, ct)
            : null;

        return new ApiResponse(vm);
    }
}