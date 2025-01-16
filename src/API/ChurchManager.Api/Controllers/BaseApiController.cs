using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => 
            (_mediator ??= HttpContext.RequestServices.GetService<IMediator>())
            ?? throw new InvalidOperationException();
    }
}
