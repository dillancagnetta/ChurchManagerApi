using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Churches.Commands.ChurchGroups;

public record AddChurchGroupCommand(string Name, string Description, int? LeaderPersonId) : IRequest<ApiResponse>;

public class AddChurchGroupCommandHandler(
    IChurchGroupService service,
    IPersonDbRepository personDb) : IRequestHandler<AddChurchGroupCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(AddChurchGroupCommand command, CancellationToken ct)
    {
        /*var dto = new ChurchGroupViewModel
        {
            Name = command.Name,
            Description = command.Description,
            LeaderPerson = command.LeaderPersonId.HasValue
                // Needed because we rerender the list - so we need this data
                ? await personDb.BasicPersonViewModelAsync(command.LeaderPersonId.Value, ct)
                : null
        };*/
        
        var dto = new EditChurchGroupModel
        {
            Name = command.Name,
            Description = command.Description,
            LeaderPersonId = command.LeaderPersonId
        };

        var vm = await service.AddAsync(dto, ct);
        
        //var entity = await service.AddAsync(dto, ct);
        // Needed because we rerender the list - so we need this data
        vm.LeaderPerson = command.LeaderPersonId.HasValue
            ? await personDb.BasicPersonViewModelAsync(command.LeaderPersonId.Value, ct)
            : null;

        return new ApiResponse(vm);
    }
}