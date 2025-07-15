using ChurchManager.Api.Middlewares;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Features.Finances.Commands;
using ChurchManager.Features.Finances.Queries;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class FinancesController : BaseApiController
{
    private readonly ILogger<FinancesController> _logger;
    private readonly IFundsService _service;
    private readonly IAppCurrentUser _currentUser;

    public FinancesController(
        ILogger<FinancesController> logger,
        IFundsService service,
        IAppCurrentUser currentUser)
    {
        _logger = logger;
        _service = service;
        _currentUser = currentUser;
    }
        
    // POST: http://localhost:5001/api/v1/finances?isDryRun=false
    [HttpPost]  
    public async Task<IActionResult> UploadBankStatement(IFormFile file, bool isDryRun = true, CancellationToken token = default)
    {
        // Add image
        var command = new UploadBankStatementCommand(file, isDryRun);
            
        var response = await Mediator.Send(command, token);
            
        return Ok(response);  
    }
    
    [HttpGet("funds")]
    [AllowAnonymous]
    [AllowedDomains]
    [EnableRateLimiting("public-endpoint")]
    public async Task<IActionResult> GetFundsTree(CancellationToken token)
    {
        return Ok(new ApiResponse(await _service.FundsWithChildren(token)));
    }
    
    [HttpPost("payment-reference")]
    [AllowAnonymous]
    [AllowedDomains]
    [EnableRateLimiting("public-endpoint")]
    public async Task<IActionResult> GetPaymentReference(GeneratePaymentReferenceCommand command, CancellationToken token)
    {
        var response = await Mediator.Send(command, token);
            
        return Ok(response);  
    }
}