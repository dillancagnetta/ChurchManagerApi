﻿using ChurchManager.Features.People.Commands.AddPersonToFamily;
using ChurchManager.Features.People.Queries.BrowseFamilies;
using ChurchManager.Features.People.Queries.GetFamily;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class FamiliesController : BaseApiController
    {
        [HttpPost("browse")]
        public async Task<IActionResult> Browse([FromBody] BrowseFamiliesQuery query, CancellationToken token)
        {
            var groups = await Mediator.Send(query, token);
            return Ok(groups);
        }

        [HttpGet("{familyId}")]
        public async Task<IActionResult> GetFamily(int familyId, [FromQuery] bool includePeople, CancellationToken token)
        {
            return Ok(await Mediator.Send(new GetFamilyQuery(familyId, includePeople), token));
        }

        [HttpPost("add-person")]
        public async Task<IActionResult> AddPersonToFamily([FromBody] AddPersonToFamilyCommand command, CancellationToken token)
        {
            await Mediator.Send(command, token);
            return Accepted();
        }
    }
}
