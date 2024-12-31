using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Groups.Queries.Reports.AttendanceMetricsComparison;

public record AttendanceMetricsComparisonQuery(int? GroupId = null, ReportPeriodType PeriodType = ReportPeriodType.SixMonths) : IRequest<ApiResponse>;

public class AttendanceMetricsComparisonHandler : IRequestHandler<AttendanceMetricsComparisonQuery, ApiResponse>
{
    private readonly IGroupAttendanceDbRepository _dbRepository;

    public AttendanceMetricsComparisonHandler(IGroupAttendanceDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }
    
    public async Task<ApiResponse> Handle(AttendanceMetricsComparisonQuery query, CancellationToken ct)
    {
        var result = await _dbRepository.GroupAttendanceMetricsComparisonAsync(query.GroupId, query.PeriodType, ct);

        return new ApiResponse(result);
    }
}