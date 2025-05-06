using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Churches.Commands;
public record AddChurchCommand(string Name, string Description, int ChurchGroupId, int? LeaderPersonId)
    : IRequest<ApiResponse>
{
    public ICollection<ChurchServiceTimeViewModel> ServiceTimes { get; set; } = [];
    public string? ShortCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}

public class AddChurchCommandHandler(
    IChurchService service,
    IPersonDbRepository personDb,
    IReadDbRepository<ChurchAttendanceType> attendanceTypeDb) : IRequestHandler<AddChurchCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(AddChurchCommand command, CancellationToken ct)
    {
        var attendanceMap = attendanceTypeDb.Queryable().AsNoTracking()
            .Where(x => command.ServiceTimes.Select(x => x.ChurchAttendanceType!).Contains(x.Name))
            .Select(x => new { x.Name , x.Id })
            .ToDictionary(x => x.Name, x => x.Id);
        
        // We need to add the attendance ids
        command.ServiceTimes.ForEach(x =>
        {
            x.ChurchAttendanceTypeId = attendanceMap[x.ChurchAttendanceType!];
        });
        
        var dto = new EditChurchModel
        {
            Name = command.Name,
            Description = command.Description,
            ShortCode = command.ShortCode,
            PhoneNumber = command.PhoneNumber,
            Address = command.Address,
            ChurchGroupId = command.ChurchGroupId,
            LeaderPersonId = command.LeaderPersonId,
            ServiceTimes = command.ServiceTimes
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