using ChurchManager.Domain.Features.Settings;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.Abstractions.Services;

public interface ISettingsService: ICrudServiceAsync<Setting, SettingViewModel, EditSettingViewModel>
{
    /// <summary>
    ///     Set setting value
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <param name="tenantName">the tenant Name</param>
    /// <param name="ct">CancellationToken</param>
    Task SetSettingAsync<T>(string key, T value, string tenantName = "", CancellationToken ct = default);
    
    /// <summary>
    ///     Save settings object
    /// </summary>
    Task SaveSettingAsync<T>(string key, T value, string tenantName = "", CancellationToken ct = default) where T : ISettings, new();
    
    Task SaveSettingAsync<T>(T value, string tenantName = "", CancellationToken ct = default) where T : ISettings, new();
    
    ISettings? LoadSetting(Type type, string tenantName = "");
    
    Task<ISettings?> LoadSettingAsync(Type type, string? tenantName, CancellationToken ct = default);
    
    Task<T> LoadSettingAsync<T>(string tenantName = "", CancellationToken ct = default) where T : ISettings, new();

    /// <summary>
    ///     Get setting value by key
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="key">Key</param>
    /// <param name="defaultValue">defaultValue</param>
    /// <param name="tenantName">tenantName</param>
    /// <returns>Setting value</returns>
    Task<T?> GetSettingByKeyAsync<T>(string key, T defaultValue = default, string tenantName = "", CancellationToken ct = default);
    
    /// <summary>
    ///     Delete all settings
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    Task DeleteSetting<T>(CancellationToken ct = default) where T : ISettings, new();
    
    /// <summary>
    ///     Delete all settings for a tenant
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    Task DeleteSetting<T>(string tenantName = "", CancellationToken ct = default) where T : ISettings, new();
}