using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Infrastructure.Plugins;

namespace Payments.PayFast;

public class PayFastPaymentPlugin(ISettingsService settingService) : BasePlugin
{
    /*public override string ConfigurationUrl()
    {
        return ExamplePluginDefaults.ConfigurationUrl;
    }*/

    public override async Task Install()
    {
        //settings
        var settings = new PayFastSettings {
          MerchantId  = "10003473",
          MerchantKey  = "gj108nu63wd7t",
          TestMode = true
        };
        await settingService.SaveSettingAsync(settings);
        
        await base.Install();
    }

    public override async Task Uninstall()
    {
        //settings
        await settingService.DeleteSetting<PayFastSettings>();
        
        await base.Uninstall();
    }
}