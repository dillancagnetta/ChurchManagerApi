using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Churches.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Churches.Queries.Reports.AttendanceMetrics;

public record ChurchAttendanceMetricsComparisonQuery(int? ChurchId = null, ReportPeriodType PeriodType = ReportPeriodType.SixMonths): IRequest<ApiResponse>;


public class ChurchAttendanceMetricsComparisonHandler : IRequestHandler<ChurchAttendanceMetricsComparisonQuery, ApiResponse>
{
    private readonly IChurchAttendanceDbRepository _dbRepository;

    public ChurchAttendanceMetricsComparisonHandler(IChurchAttendanceDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }
    
    public async Task<ApiResponse> Handle(ChurchAttendanceMetricsComparisonQuery query, CancellationToken ct)
    {
        var result = await _dbRepository.AttendanceMetricsComparisonAsync(query.ChurchId, query.PeriodType, ct);

        return new ApiResponse(result);
    }
}