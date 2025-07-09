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

    public override async Task Install()
    {
        //settings
        var settings = new ExamplePluginSettings {
            Setting1 = true,
            Setting2 = "Test Example Setting"
        };
        await settingService.SaveSettingAsync(settings);
    }

    public override async Task Uninstall()
    {
        //settings
        await settingService.DeleteSetting<ExamplePluginSettings>();
    }
}