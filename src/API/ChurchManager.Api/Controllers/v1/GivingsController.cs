using ChurchManager.Features.Finances.Queries;
using ChurchManager.SharedKernel.Common;
using CodeBoss.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class GivingsController : BaseApiController
{
    private readonly ILogger<GivingsController> _logger;
    private readonly IAppCurrentUser _currentUser;

    public GivingsController(
        ILogger<GivingsController> logger,
        IAppCurrentUser currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }
        
        
    [HttpPost("browse")]
    public async Task<IActionResult> Browse([FromBody] BrowseGivingsQuery query, CancellationToken token)
    {
        var isFamilyGiving = User.IsInRole("Public Access");

        if (isFamilyGiving)
        {
            var familyId = User.Claims.FirstOrDefault(c => c.Type == "FamilyId")?.Value.AsIntegerOrNull() 
                           ?? throw new InvalidOperationException("FamilyId claim not found");

            var familyQuery = new BrowseFamilyGivingsQuery
            {
                FamilyId = familyId,
                From = query.From,
                To = query.To,
                Person = query.Person,
                SearchTerm = query.SearchTerm,
                GivingTypes = query.GivingTypes,
                PaymentMethods = query.PaymentMethods,
                Currency = query.Currency,
                FundId = query.FundId,
                BenefactorTypes = query.BenefactorTypes
            };
            
            return Ok(await Mediator.Send(familyQuery, token));
        }

        // BrowseGivingsQuery
        return Ok(await Mediator.Send(query, token));
    }
}