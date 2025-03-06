using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Churches.Specifications;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Churches.Queries.BrowseAttendance;

public record ChurchAttendanceReportGridQuery : IRequest<ApiResponse>
{
    public int[] AttendanceTypeIds { get; set; }
    public int? ChurchGroupId { get; set; }
    public int[] ChurchIds { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class ChurchAttendanceReportGridHandler(IReadDbRepository<ChurchAttendance> dbRepository) : IRequestHandler<ChurchAttendanceReportGridQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(ChurchAttendanceReportGridQuery query, CancellationToken ct)
    {
        var spec = new ChurchAttendanceReportGridSpecification(query.AttendanceTypeIds, query.ChurchGroupId, query.ChurchIds, query.From, query.To);

            var results = await dbRepository.ListAsync<ChurchAttendanceViewModel>(spec, ct);

        return new ApiResponse(results);
    }
}