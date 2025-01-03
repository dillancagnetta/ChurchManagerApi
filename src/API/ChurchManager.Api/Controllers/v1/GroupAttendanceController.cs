using ChurchManager.Features.Groups.Commands.DeleteGroupAttendanceRecord;
using ChurchManager.Features.Groups.Queries.GroupAttendanceRecordSubmissions;
using ChurchManager.Features.Groups.Queries.GroupMemberAttendance;
using ChurchManager.Features.Groups.Queries.Reports.AttendanceMetricsComparison;
using ChurchManager.Features.Groups.Queries.Reports.AttendanceReportGrid;
using ChurchManager.Features.Groups.Queries.Reports.MemberAttendanceGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class GroupAttendanceController : BaseApiController
    {
        [HttpDelete("{groupAttendanceId}")]
        public async Task<IActionResult> DeleteAttendanceRecord(int groupAttendanceId, CancellationToken token)
        {
            var data = await Mediator.Send(new DeleteGroupAttendanceRecordCommand(groupAttendanceId), token);
            return Ok(data);
        }


        [HttpPost("attendance-report-grid")]
        public async Task<IActionResult> AttendanceReportGrid([FromBody] AttendanceReportGridQuery query, CancellationToken token)
        {
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }

        [HttpPost("report-submissions")]
        public async Task<IActionResult> GroupAttendanceRecordSubmissions([FromBody] GroupAttendanceRecordSubmissionsQuery query, CancellationToken token)
        {
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }

        [HttpGet("members-attendance")]
        public async Task<IActionResult> GroupMembersAttendance([FromQuery] GroupMembersAttendanceQuery query, CancellationToken token)
        {
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }
        
        [HttpGet("members-attendance-report")]
        public async Task<IActionResult> GroupMembersAttendanceReport([FromQuery] MemberAttendanceGridQuery query, CancellationToken token)
        {
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }
        
        [HttpGet("attendance-metrics-comparison")]
        public async Task<IActionResult> GroupAttendanceMetricsComparison([FromQuery] AttendanceMetricsComparisonQuery query, CancellationToken token)
        {
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }
        
        [HttpGet("group-annual-conversion-rate-comparison")]
        public async Task<IActionResult> GroupYearlyConversionRateComparison([FromQuery] GroupYearlyConversionRateComparisonQuery query,CancellationToken token = default)
        {
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }
        
        [HttpPost("groups-average-attendance-rate")]
        public async Task<IActionResult> AverageAttendanceRateForGroups([FromBody] GroupsAverageAttendanceRateQuery query,CancellationToken token = default)
        {
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }
    }
}
