using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Repositories;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class MessagesController(
    ICognitoCurrentUser currentUser,
    IMessageDbRepository dbRepository) : BaseApiController
{
    [HttpGet("current-user")]
    public async Task<IActionResult> GetCurrentUserMessages(CancellationToken ct)
    {
        var messages = await dbRepository.AllAsync(currentUser.Id.AsGuid(), MessageStatus.Pending, null, ct);
        return Ok(new ApiResponse(messages));
    }
    
    [HttpPut("{messageId}")]
    public async Task<IActionResult> MarkAsRead(int messageId, CancellationToken ct)
    {
        await dbRepository.MarkAsReadAsync(messageId, ct);
        return Ok();
    }
    
    [HttpDelete("{messageId}")]
    public async Task<IActionResult> DeleteMessage(int messageId, CancellationToken ct)
    {
        await dbRepository.DeleteAsync(messageId, ct);
        return Ok();
    }
}