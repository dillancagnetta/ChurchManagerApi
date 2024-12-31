using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Groups.Queries.Reports.MemberAttendanceGrid;

public record MemberAttendanceGridQuery : IRequest<ApiResponse>
{
    public int GroupId { get; set; }
}

public class MemberAttendanceReportGridHandler : IRequestHandler<MemberAttendanceGridQuery, ApiResponse>
{
    private readonly IGroupMemberAttendanceDbRepository _dbRepository;

    public MemberAttendanceReportGridHandler(IGroupMemberAttendanceDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task<ApiResponse> Handle(MemberAttendanceGridQuery query, CancellationToken ct)
    {
        var results = await _dbRepository.MemberAttendanceReportAsync(query.GroupId, ct);
        // Order by names
        return new ApiResponse(results.OrderByDescending(x => x.PersonName));
    }
}