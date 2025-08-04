using System.Text.Json;

namespace ChurchManager.Domain.Features.Settings;

public static class SettingExtensions
{
    public static Setting CreateSetting<T>(
        this T settings, 
        string? key = null
        ) where T : ISettings, new()
    {
        key ??= typeof(T).Name;
        var setting = new Setting {
            Name = key.Trim().ToLowerInvariant(),
            Metadata = JsonSerializer.Serialize(settings),
        };
        return setting;
    }
}
