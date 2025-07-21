using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Infrastructure.Abstractions.AppContext;
using ChurchManager.Infrastructure.Plugins;

namespace Payments.PayFast;

public class PayFastPaymentPlugin(
    ISettingsService settingService,
    IAppContextAccessor contextAccessor) : BasePlugin
{
    /*public override string ConfigurationUrl()
    {
        return ExamplePluginDefaults.ConfigurationUrl;
    }*/

    public override async Task InstallAsync(CancellationToken ct = default)
    {
        //settings
        var settings = new PayFastSettings {
          MerchantId  = "10003473",
          MerchantKey  = "gj108nu63wd7t",
          Passphrase = "pancakesaregreat",
          CancelUrl = $"public/giving/cancel",
          ReturnUrl = $"public/giving/thank-you",
          NotifyUrl = $"payments/payfast/notify",
          TestMode = true
        };
        await settingService.SaveSettingAsync(
            settings,
            tenantName:contextAccessor.AppContext.CurrentTenant.Name,
            ct: ct);
        
        await base.InstallAsync(ct);
    }

    public override async Task UninstallAsync(CancellationToken ct = default)
    {
        //settings
        await settingService.DeleteSetting<PayFastSettings>(
            tenantName:contextAccessor.AppContext.CurrentTenant.Name, ct);
        
        await base.UninstallAsync(ct);
    }
}