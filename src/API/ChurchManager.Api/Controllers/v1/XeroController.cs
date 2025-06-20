using ChurchManager.Infrastructure.Abstractions.Finances;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
public class XeroController(
    IExternalFinanceIntegrator integrator,
    ILogger<XeroController> logger): BaseApiController
{
 
    // GET /Authorization/Callback
    /// <summary>
    /// Callback validating returned data to prevent cross site forgey attacks
    /// </summary>
    /// <param name="code">Returned code</param>
    /// <param name="state">Returned state</param>
    /// <returns>Redirect to organisations page</returns>
    [HttpGet("callback")]
    public async Task<IActionResult> Callback(string code)
    {
        var accessToken = await integrator.RequestAccessTokenAsync(code);
        return Ok();
    }
}