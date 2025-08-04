using System.Text.Json;
using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.Features;
using ChurchManager.Domain.Features.Settings;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Settings.Services;

public class SettingsService(
    IGenericDbRepository<Setting> repository, 
    IMapper mapper
    )
    : CrudServiceAsync<Setting, SettingViewModel, EditSettingViewModel>(repository, mapper), ISettingsService
{
    public async Task SaveSettingAsync<T>(T value, string? key = null, CancellationToken ct = default)
        where T : ISettings, new()
    {
        key ??= typeof(T).Name;

        var query = SettingsByNameQuery(key);
        var setting = await query.FirstOrDefaultAsync(ct);

        var metadata = JsonSerializer.Serialize(value);

        if (setting != null)
        {
            setting.Metadata = metadata;
            await repository.UpdateAsync(setting, ct);
        }
        else
        {
            setting = value.CreateSetting(key);
            await repository.AddAsync(setting, ct);
        }

        await repository.SaveChangesAsync(ct);
    }

    
    public virtual ISettings? LoadSetting(Type type)
    {
        var query =  SettingsByNameQuery(type.Name);
        var setting = query.FirstOrDefault();

        return TrySerializeSettings(type, setting);
    }

    public async Task<ISettings?> LoadSettingAsync(Type type, string? key = null, CancellationToken ct = default)
    {
        key ??= type.Name;
        
        IQueryable<Setting> query = Repository.Queryable().AsNoTracking();
        
        var name = key.ToLowerInvariant();
        query = query.Where(x => x.Name == name);
        
        var setting = await query.FirstOrDefaultAsync(ct);
        
        return TrySerializeSettings(type, setting);
    }

    public virtual Task<T?> LoadSettingAsync<T>(CancellationToken ct = default) where T : ISettings, new()
    {
         return Task.FromResult((T)LoadSetting(typeof(T)));
    }

    public virtual async Task<T?> GetSettingByKeyAsync<T>(string key, T? defaultValue = default, CancellationToken ct = default)
    {
        if (key.IsNullOrEmpty()) return defaultValue;
        
        var query =  SettingsByNameQuery(key);
        
        var setting = await query.FirstOrDefaultAsync(ct);
    
        return setting != null ? JsonSerializer.Deserialize<T>(setting.Metadata) : defaultValue;
    }

    public async Task DeleteSetting<T>(CancellationToken ct = default) where T : ISettings, new()
    {
        var query = SettingsByNameQuery(typeof(T).Name);
        var settings = await query.ToListAsync(ct);

        foreach (var setting in settings)
        {
            await repository.DeleteAsync(setting, ct);
        }
        await repository.SaveChangesAsync(ct);
    }

    private IQueryable<Setting> SettingsByNameQuery(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        
        name = name.Trim().ToLowerInvariant();
        return Repository.Queryable().Where(x => x.Name == name);
    }
    
    /*private IQueryable<Setting> SettingsFilterQuery(IQueryable<Setting> query, 
        int? churchGroupId = null, int? churchId = null , int? personId = null)
    {
        if (churchGroupId.HasValue)
        {
            query= query.Where(x => x.ChurchGroupId == churchGroupId);
        }
        if (churchId.HasValue)
        {
            query=  query.Where(x => x.ChurchId == churchId);
        }
        if (personId.HasValue)
        {
            query=query.Where(x => x.PersonId == personId);
        }
        
        return query;
    }*/
    
    private ISettings? TrySerializeSettings(Type type, Setting? setting)
    {
        if (setting == null) return Activator.CreateInstance(type) as ISettings;
        
        var _setting = JsonSerializer.Deserialize(setting.Metadata, type) as ISettings;

        return _setting ?? Activator.CreateInstance(type) as ISettings;
    }
}