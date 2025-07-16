
using ChurchManager.Infrastructure.Plugins;
using Payments.PayFast;

[assembly: PluginInfo(
    FriendlyName = PayFastPluginDefaults.FriendlyName,
    Group = "Payments",
    SystemName = PayFastPluginDefaults.ProviderSystemName,
    Author = "ChurchManager Team",
    Version = "1.0.0"
)]