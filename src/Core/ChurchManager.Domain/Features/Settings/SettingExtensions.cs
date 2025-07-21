using System.Text.Json;

namespace ChurchManager.Domain.Features.Settings;

public static class SettingExtensions
{
    public static Setting CreateSetting<T>(
        T settings, 
        string tenantName = ""
        ) where T : ISettings, new()
    {
        var setting = new Setting {
            Name = typeof(T).Name.ToLowerInvariant(),
            Metadata = JsonSerializer.Serialize(settings),
            TenantName = tenantName.ToLower()
        };
        return setting;
    }
}
