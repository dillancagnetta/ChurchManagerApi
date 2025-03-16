using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
    public string AgeClassification { get; set; }
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
            IconCssClass = command.IconCssClass,
            OnlineSupport = command.OnlineSupport,
            RequiresChildInfo = command.RequiresChildInfo,
            RequiresRegistration = command.RequiresRegistration,
            AllowFamilyRegistration = command.AllowFamilyRegistration,
            AllowNonFamilyRegistration = command.AllowNonFamilyRegistration,
            TakesAttendance = command.TakesAttendance,
            ChildCare = command.HasChildCare ? new ChildCare
            {
                HasChildCare = command.HasChildCare,
                MinChildAge = command.MinChildAge,
                MaxChildAge = command.MaxChildAge,
            } : null,
            AgeClassification = command.AgeClassification,
        };

        var vm = await service.AddAsync(entity, ct);
        
        // Needed because we rerender the list - so we need this data
        vm.GroupTypeName = command.GroupTypeId.HasValue
            ? await groupTypesDb.Queryable()
                .Where(x => x.Id == command.GroupTypeId.Value)
                .Select(x => x.Name)
                .FirstOrDefaultAsync(cancellationToken: ct)
            : null;

        return new ApiResponse(vm);
    }
}