using ChurchManager.Api.Authorization.AllowTesting;
using ChurchManager.Infrastructure.Abstractions.Plugins;
using ChurchManager.Infrastructure.Plugins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class PluginsController(
    IServiceProvider serviceProvider,
    ILogger<PluginsController> logger) : BaseApiController
{
    [HttpPost("install-all")]
    [AllowTesting]
    public async Task<IActionResult> InstallAll(CancellationToken ct)
    {
        PluginManager.ClearPlugins();
        
        var pluginsInfo = PluginManager.ReferencedPlugins?.ToList() ?? [];
        
        foreach (var pluginInfo in pluginsInfo)
            try
            {
                var plugin = pluginInfo.Instance<IPlugin>(HttpContext.RequestServices.CreateScope().ServiceProvider);
                await plugin!.InstallAsync(ct);
                logger.LogInformation($"Plugin {plugin.PluginInfo.FriendlyName} has been installed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during installing plugin " + pluginInfo.SystemName,
                    ex.Message + " " + ex.InnerException?.Message);
            }
        
        return Ok();
    }
    
    [HttpDelete]
    [AllowTesting]
    public async Task<IActionResult> Uninstall(string systemName, CancellationToken ct)
    {
        try
        {
            var pluginInfo = PluginManager.ReferencedPlugins!.FirstOrDefault(x => x.SystemName == systemName);
            if (pluginInfo == null) return NotFound("No Plugin found with the provided system name");

            //check whether plugin is installed
            if (!pluginInfo.Installed) return Ok("This plugin is not installed");

            //uninstall plugin
            var plugin = pluginInfo.Instance<IPlugin>(serviceProvider);
            await plugin!.UninstallAsync(ct);
            
            logger.LogInformation("The plugin has been uninstalled: {pluginName}", systemName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during uninstalling plugin: {pluginName} ", systemName);
            return BadRequest("Error during uninstalling plugin");
        }

        return Ok();
    }

       
}