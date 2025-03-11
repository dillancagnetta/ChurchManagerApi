using ChurchManager.Features.Churches.Commands.Attendance;
using ChurchManager.Features.Churches.Queries.BrowseAttendance;
using ChurchManager.Features.Churches.Queries.RetrieveChurches;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class ChurchesController : BaseApiController
{
    private readonly ICognitoCurrentUser _currentUser;

    public ChurchesController(ICognitoCurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    #region CRUD

    [HttpGet]
    public async Task<IActionResult> AllChurches(int? churchGroupId, CancellationToken token)
    {
        var groups = await Mediator.Send(new ChurchesQuery
        {
            ChurchGroupId = churchGroupId
        }, token);
        return Ok(groups);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddChurchCommand cmd, CancellationToken token)
    {
        return Ok(await Mediator.Send(cmd, token));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken token)
    {
        return Ok(await Mediator.Send(new DeleteChurchCommand(id), token));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] EditChurchCommand cmd, CancellationToken token)
    {
        return Accepted(await Mediator.Send(cmd, token));
    }

    #endregion

    #region Attendance

    [HttpPost("attendance/browse")]
    public async Task<IActionResult> BrowseChurchAttendances([FromBody] BrowseChurchAttendanceQuery query, CancellationToken token)
    {
        var attendances = await Mediator.Send(query, token);
        return Ok(attendances);
    }
    
    [HttpPost("attendance-report-grid")]
    public async Task<IActionResult> AttendanceReportGrid([FromBody] ChurchAttendanceReportGridQuery query, CancellationToken token)
    {
        var data = await Mediator.Send(query, token);
        return Ok(data);
    }
    
    [HttpPost("submit-attendance")]
    public async Task<IActionResult> SubmitAttendanceRecord([FromBody] SubmitChurchAttendanceCommand command, CancellationToken token)
    {
        var attendances = await Mediator.Send(command, token);
        return Ok(attendances);
    }

    #endregion
    
   
}