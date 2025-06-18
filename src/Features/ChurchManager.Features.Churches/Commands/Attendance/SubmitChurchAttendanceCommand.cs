using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Churches.Commands.Attendance;

public record SubmitChurchAttendanceCommand : IRequest<ApiResponse>
{
    public DateTime AttendanceDate { get; set; }
    public int AttendanceTypeId { get; set; }
    public int ChurchId { get; set; }
    public bool DidNotOccur { get; set; } = false;
    public string Notes { get; set; } = string.Empty;
    public int? FirstTimerCount { get; set; }
    public int? NewConvertCount { get; set; }
    public int? ReceivedHolySpiritCount { get; set; }
    public int? MalesCount { get; set; }
    public int? FemalesCount { get; set; }
    public int? ChildrenCount { get; set; }
    public int? TeensCount { get; set; }
}

public class SubmitChurchAttendanceHandler(IGenericDbRepository<ChurchAttendance> dbRepository) : IRequestHandler<SubmitChurchAttendanceCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(SubmitChurchAttendanceCommand command, CancellationToken ct)
    {
        var entity = new ChurchAttendance
        {
            ChildrenCount = command.ChildrenCount,
            TeensCount = command.TeensCount,
            FemalesCount = command.FemalesCount,
            FirstTimerCount = command.FirstTimerCount,
            MalesCount = command.MalesCount,
            NewConvertCount = command.NewConvertCount,
            ReceivedHolySpiritCount = command.ReceivedHolySpiritCount,
            AttendanceDate = command.AttendanceDate,
            ChurchAttendanceTypeId = command.AttendanceTypeId,
            ChurchId = command.ChurchId,
            DidNotOccur = command.DidNotOccur,
            Notes = command.Notes
        };
        
        var results = await dbRepository.AddAsync(entity, ct);

        return new ApiResponse(results);
    }
}