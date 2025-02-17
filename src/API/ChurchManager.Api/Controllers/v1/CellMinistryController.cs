﻿using ChurchManager.Features.Groups.Commands.GroupAttendanceFeedback;
using ChurchManager.Features.Groups.Queries.BrowseGroupAttendance;
using ChurchManager.Features.Groups.Queries.Charts.Dashboard;
using ChurchManager.Features.Groups.Queries.Charts.WeeklyComparison;
using ChurchManager.Features.Groups.Queries.Reports.AttendanceMetricsComparison;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class CellMinistryController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;

        public CellMinistryController(ICognitoCurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpPost("attendance/browse")]
        public async Task<IActionResult> BrowseGroupAttendances([FromBody] BrowseGroupAttendanceQuery query, CancellationToken token)
        {
            var groups = await Mediator.Send(query, token);
            return Ok(groups);
        }

        [HttpGet("attendance/{attendanceId}")]
        public async Task<IActionResult> GetAttendanceRecord(int attendanceId, CancellationToken token)
        {
            var data = await Mediator.Send(new AttendanceRecordQuery(attendanceId), token);
            return Ok(data);
        }

        [HttpPost("attendance/feedback")]
        public async Task<IActionResult> AttendanceFeedback([FromBody] SubmitGroupAttendanceFeedbackCommand command, CancellationToken token)
        {
            await Mediator.Send(command, token);
            return Accepted();
        }

        [HttpGet("charts")]
        public async Task<IActionResult> Charts([FromQuery] WeeklyBreakdownForMonthQuery query, CancellationToken token)
        {
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard(CancellationToken token)
        {
            var data = await Mediator.Send(new CellMinistryDashboardMetrics(), token);
            return Ok(data);
        }
        
        [HttpGet("dashboard-annual-conversion-rate")]
        public async Task<IActionResult> DashboardYearlyConversionRateComparison([FromQuery] GroupYearlyConversionRateComparisonQuery query, CancellationToken token)
        {
            query.GroupTypeId = 2; //TODO: Get cell group type from cache or database
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }

    }
}
