using ChurchManager.Domain.Features.Settings;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.Abstractions.Services;

public interface ISettingsService: ICrudServiceAsync<Setting, SettingViewModel, EditSettingViewModel>
{
    /// <summary>
    ///     Save settings object
    /// </summary>
    Task SaveSettingAsync<T>(T value, string? key = null, CancellationToken ct = default) where T : ISettings, new();
    
    ISettings? LoadSetting(Type type);
    
    Task<ISettings?> LoadSettingAsync(Type type, string? key = null, CancellationToken ct = default);
    
    Task<T> LoadSettingAsync<T>(CancellationToken ct = default) where T : ISettings, new();

    /// <summary>
    ///     Get setting value by key
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="key">Key</param>
    /// <param name="defaultValue">defaultValue</param>
    /// <returns>Setting value</returns>
    Task<T?> GetSettingByKeyAsync<T>(string key, T? defaultValue = default, CancellationToken ct = default);
    
    /// <summary>
    ///     Delete all settings
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    Task DeleteSetting<T>(CancellationToken ct = default) where T : ISettings, new();
}