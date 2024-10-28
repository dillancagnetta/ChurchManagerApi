using ChurchManager.Domain.Features.Churches.Repositories;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class DashboardController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;
        private readonly IChurchAttendanceDbRepository _attendanceDbRepository;
        private readonly IPersonDbRepository _personDbRepository;

        public DashboardController(
            ICognitoCurrentUser currentUser,
            IChurchAttendanceDbRepository attendanceDbRepository,
            IPersonDbRepository personDbRepository)
        {
            _currentUser = currentUser;
            _attendanceDbRepository = attendanceDbRepository;
            _personDbRepository = personDbRepository;
        }

        [HttpGet("church-attendance")]
        public async Task<IActionResult> ChurchAttendance([FromQuery] DateTime from, DateTime to, int? churchId, CancellationToken token)
        {
            var attendances =
                await _attendanceDbRepository.DashboardChurchAttendanceAsync(from, to, churchId);
            return Ok(attendances);
        }

        [HttpGet("church-attendance-breakdown")]
        public async Task<IActionResult> ChurchAttendanceBreakdown([FromQuery] DateTime from, DateTime to, CancellationToken token)
        {
            var breakdown = await _attendanceDbRepository.DashboardChurchAttendanceBreakdownAsync(from, to);
            return Ok(breakdown);
        }
        
        [HttpGet("church-people-connectionstatus-breakdown")]
        public async Task<IActionResult> ChurchConnectionStatusBreakdown([FromQuery] int? churchId, CancellationToken token)
        {
            var breakdown = await _personDbRepository.DashboardChurchConnectionStatusBreakdown(churchId);
            return Ok(breakdown);
        }
    }
}
