using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Events.Commands;

public record AddEventTypeCommand : IRequest<ApiResponse>
{
    public required string Name { get; set; }
    public string? Description { get; set; } 
    public int? DefaultGroupTypeId { get; set; }
    public string OnlineSupport { get; set; } = Domain.Features.Events.OnlineSupport.Unknown.Value;
    public bool RequiresRegistration { get; set; }
    public bool AllowFamilyRegistration { get; set; }
    public bool AllowNonFamilyRegistration { get; set; }
    public bool RequiresChildInfo { get; set; }
    public bool TakesAttendance { get; set; } = false;
    public bool? HasChildCare { get; set; }
    public int? MinChildAge { get; set; }
    public int? MaxChildAge { get; set; }
    public string? IconCssClass { get; set; }
    public string AgeClassification { get; set; } = Domain.Features.People.AgeClassification.Unknown.Value;
};

public class AddEventTypeCommandHandler(
    IEventTypeService service,
    IReadDbRepository<GroupType> groupTypesDb,
    IMapper mapper) : IRequestHandler<AddEventTypeCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(AddEventTypeCommand command, CancellationToken ct)
    {
        var entity = new EventType()
        {
            Name = command.Name,
            Description = command.Description,
            DefaultGroupTypeId = command.DefaultGroupTypeId,
            IconCssClass = command.IconCssClass,
            OnlineSupport = command.OnlineSupport,
            RequiresChildInfo = command.RequiresChildInfo,
            RequiresRegistration = command.RequiresRegistration,
            AllowFamilyRegistration = command.AllowFamilyRegistration,
            AllowNonFamilyRegistration = command.AllowNonFamilyRegistration,
            TakesAttendance = command.TakesAttendance,
            ChildCare = command.HasChildCare.HasValue ? new ChildCare
            {
                HasChildCare = command.HasChildCare.Value,
                MinChildAge = command.MinChildAge,
                MaxChildAge = command.MaxChildAge,
            } : null,
            AgeClassification = command.AgeClassification,
        };

        var vm = await service.AddAsync(entity, ct);
        
        // Needed because we rerender the list - so we need this data
        vm.GroupTypeName = command.DefaultGroupTypeId.HasValue
            ? await groupTypesDb.Queryable()
                .Where(x => x.Id == command.DefaultGroupTypeId.Value)
                .Select(x => x.Name)
                .FirstOrDefaultAsync(cancellationToken: ct)
            : null;

        return new ApiResponse(vm);
    }
}

// ----------------------------------------------------------

public record EditEventTypeCommand : AddEventTypeCommand;

public class EditEventTypeCommandHandler(
    IEventTypeService service,
    IReadDbRepository<GroupType> groupTypesDb,
    IMapper mapper) : IRequestHandler<EditEventTypeCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(EditEventTypeCommand command, CancellationToken ct)
    {
        var dto = mapper.Map<EditEventTypeModel>(command);
        
        var vm = await service.UpdateAsync(dto, ct);
        
        // Needed because we rerender the list - so we need this data
        vm.GroupTypeName = command.DefaultGroupTypeId.HasValue
            ? await groupTypesDb.Queryable()
                .Where(x => x.Id == command.DefaultGroupTypeId.Value)
                .Select(x => x.Name)
                .FirstOrDefaultAsync(cancellationToken: ct)
            : null;

        return new ApiResponse(vm);
    }
}