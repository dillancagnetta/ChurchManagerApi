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

/*
 * ---------------------------------------------------------------
 */

public record GroupYearlyConversionRateComparisonQuery : IRequest<ApiResponse>
{
    public int? GroupTypeId { get; set; }
    public int? ChurchId { get; set; }
    public int? GroupId { get; set; }
    public bool IncludeMonthlyBreakdown { get; set; } = false;
};

public class GroupYearlyConversionRateComparisonHandler : IRequestHandler<GroupYearlyConversionRateComparisonQuery, ApiResponse>
{
    private readonly IGroupAttendanceDbRepository _dbRepository;

    public GroupYearlyConversionRateComparisonHandler(IGroupAttendanceDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }
    
    public async Task<ApiResponse> Handle(GroupYearlyConversionRateComparisonQuery query, CancellationToken cancellationToken)
    {
        var result = await _dbRepository.YearlyConversionComparisonAsync(
            groupTypeId: query.GroupTypeId.Value,
            churchId: query.ChurchId,
            groupId: query.GroupId,
            includeMonthlyBreakdown: query.IncludeMonthlyBreakdown,
            cancellationToken);
        
        return new ApiResponse(result);
    }
}
