using System.Text.RegularExpressions;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Group = ChurchManager.Domain.Features.Groups.Group;

namespace ChurchManager.Features.Events.Commands;

public record AddEventTypeCommand : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public string Description { get; set; } 
    public int? GroupTypeId { get; set; }
    public string OnlineSupport { get; set; } 
    public bool RequiresRegistration { get; set; }
    public bool AllowFamilyRegistration { get; set; }
    public bool AllowNonFamilyRegistration { get; set; }
    public bool RequiresChildInfo { get; set; }
    public bool TakesAttendance { get; set; }
    public bool HasChildCare { get; set; }
    public int? MinChildAge { get; set; }
    public int? MaxChildAge { get; set; }
    public string IconCssClass { get; set; }
};

public class AddEventTypeCommandHandler(
    IEventTypeService service,
    IReadDbRepository<GroupType> groupTypesDb) : IRequestHandler<AddEventTypeCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(AddEventTypeCommand command, CancellationToken ct)
    {
        var dto = new EditEventTypeModel
        {
            
        };

        var vm = await service.AddAsync(dto, ct);
        
        //var entity = await service.AddAsync(dto, ct);
        // Needed because we rerender the list - so we need this data
        vm.GroupTypeName = command.GroupTypeId.HasValue
            ? await groupTypesDb.Queryable().FirstOrDefaultAsync(x => x.Id == command.GroupTypeId.Value, ct)
                .Select(x => x.Name)
            : null;

        return new ApiResponse(vm);
    }
}