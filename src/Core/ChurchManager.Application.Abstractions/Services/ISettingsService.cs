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
    /// <param name="churchGroupId">church Group identifier</param>
    /// <param name="churchId"></param>
    /// <param name="personId"></param>
    Task SetSettingAsync<T>(string key, T value, 
        int? churchGroupId = null, int? churchId = null , int? personId = null,
        CancellationToken ct = default);
    
    /// <summary>
    ///     Save settings object
    /// </summary>
    Task SaveSettingAsync<T>(string key, T value, 
        int? churchGroupId = null, int? churchId = null , int? personId = null,
        CancellationToken ct = default);
    
    ISettings LoadSetting(Type type, 
        int? churchGroupId = null, int? churchId = null , int? personId = null,
        CancellationToken ct = default);
    
    Task<T> LoadSettingAsync<T>(
        int? churchGroupId = null, int? churchId = null , int? personId = null,
        CancellationToken ct = default) where T : ISettings, new();
    
    /// <summary>
    ///     Get setting value by key
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="key">Key</param>
    /// <param name="churchGroupId">church Group identifier</param>
    /// <param name="churchId"></param>
    /// <param name="personId"></param>
    /// <param name="defaultValue">Default value</param>
    /// <returns>Setting value</returns>
    Task<T> GetSettingByKeyAsync<T>(string key, T defaultValue = default, 
        int? churchGroupId = null, int? churchId = null , int? personId = null,
        CancellationToken ct = default);
}