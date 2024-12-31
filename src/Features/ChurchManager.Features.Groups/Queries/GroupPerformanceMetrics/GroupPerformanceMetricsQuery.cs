using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Shared;
using ChurchManager.Features.Groups.Queries.GroupMemberAttendance;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Groups.Queries.GroupPerformanceMetrics
{
    public record GroupPerformanceMetricsQuery : IRequest<ApiResponse>
    {
        public int GroupId { get; set; }
        public PeriodType Period { get; set; }
    }

    public class GroupPerformanceMetricsHandler : IRequestHandler<GroupPerformanceMetricsQuery, ApiResponse>
    {
        private readonly IGroupMemberAttendanceDbRepository _dbRepository;
        private readonly IGroupAttendanceDbRepository _groupAttendanceDb;
        private readonly IGroupMemberDbRepository _groupMemberDb;
        private readonly IMediator _mediator;

        public GroupPerformanceMetricsHandler(
            IGroupMemberAttendanceDbRepository dbRepository,
            IGroupAttendanceDbRepository groupAttendanceDb,
            IGroupMemberDbRepository groupMemberDbRepository,
            IMediator mediator)
        {
            _dbRepository = dbRepository;
            _groupAttendanceDb = groupAttendanceDb;
            _groupMemberDb = groupMemberDbRepository;
            _mediator = mediator;
        }

        public async Task<ApiResponse> Handle(GroupPerformanceMetricsQuery query, CancellationToken ct)
        {
            //var attendanceRecords = (await _mediator.Send(new GroupAttendanceQuery(query.GroupId, query.Period), ct)).Data as GroupMembersAttendanceAnalysisViewModel;
            var avgAttendanceRate = (await _mediator.Send(new GroupsAverageAttendanceRateQuery([query.GroupId], query.Period), ct));

            #region Metrics
            
            var (newConvertCount, firstTimerCount, holySpiritCount) = await _groupAttendanceDb.PeopleStatisticsAsync([query.GroupId], query.Period, ct);
            var (members, leaders) = (await _groupMemberDb.PeopleAndLeadersInGroupAsync(query.GroupId, ct));
            var metrics = new
            {
                firstTimerCount,
                newConvertCount,
                holySpiritCount,
                membersCount = members + leaders,
                assistantsCount = leaders > 1 ? leaders - 1 : 0, // subtract main leader from assistants
                avgAttendanceRate = avgAttendanceRate.FirstOrDefault()?.AverageAttendanceRatePercent
            };
            
            #endregion

            return new ApiResponse( new { metrics });
        }
    }
}
