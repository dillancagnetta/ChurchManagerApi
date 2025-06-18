using System.Text.Json;

namespace ChurchManager.Domain.Features.Settings;

public static class SettingExtensions
{
    public static Setting CreateSetting<T>(
        T settings, 
        int? churchGroupId = null,
        int? churchId = null,
        int? personId = null
        ) where T : ISettings, new()
    {
        var setting = new Setting {
            Name = typeof(T).Name.ToLowerInvariant(),
            Metadata = JsonSerializer.Serialize(settings),
            ChurchGroupId = churchGroupId,
            ChurchId = churchId,
            PersonId = personId
        };
        return setting;
    }
}
