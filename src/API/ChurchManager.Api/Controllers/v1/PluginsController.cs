using ChurchManager.Api.Authorization.AllowTesting;
using ChurchManager.Infrastructure.Plugins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class PluginsController(ILogger<PluginsController> logger) : BaseApiController
{
    [HttpPost("install-all")]
    [AllowTesting]
    public async Task<IActionResult> InstallAll(CancellationToken token)
    {
        PluginManager.ClearPlugins();
        
        var pluginsInfo = PluginManager.ReferencedPlugins?.ToList() ?? [];
        
        foreach (var pluginInfo in pluginsInfo)
            try
            {
                var plugin = pluginInfo.Instance<IPlugin>(HttpContext.RequestServices.CreateScope().ServiceProvider);
                await plugin!.Install();
                logger.LogInformation($"Plugin {plugin.PluginInfo.FriendlyName} has been installed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during installing plugin " + pluginInfo.SystemName,
                    ex.Message + " " + ex.InnerException?.Message);
            }
        
        return Ok();
    }

       
}