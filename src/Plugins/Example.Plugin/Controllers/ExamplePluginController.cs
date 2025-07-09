using Microsoft.AspNetCore.Mvc;

namespace Example.Plugin.Controllers;

[ApiController]
[Route("[controller]")]
public class ExamplePluginController : ControllerBase
{
    [HttpGet]
    public IActionResult HealthCheck()
    {
        return Ok("All good in PLUGIN hood!");
    }
}