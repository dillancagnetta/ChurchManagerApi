using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Infrastructure.Plugins;

namespace Example.Plugin;

public class ExamplePlugin(
    ISettingsService settingService) : BasePlugin
{
    /*public override string ConfigurationUrl()
    {
        return ExamplePluginDefaults.ConfigurationUrl;
    }*/

    public override async Task InstallAsync(CancellationToken ct = default)
    {
        //settings
        var settings = new ExamplePluginSettings {
            Setting1 = true,
            Setting2 = "Test Example Setting"
        };
        await settingService.SaveSettingAsync(settings, ct: ct);
    }

    public override async Task UninstallAsync(CancellationToken ct = default)
    {
        //settings
        await settingService.DeleteSetting<ExamplePluginSettings>(ct);
    }
}