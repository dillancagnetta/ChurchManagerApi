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
    public virtual async Task SetSettingAsync<T>(string key, T value, string tenantName = "",
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        
        key = key.Trim().ToLowerInvariant();
        var query =  SettingsByNameQuery(key);
        query = SettingsByTenantQuery(query, tenantName);
        
        var setting = await query.FirstOrDefaultAsync(ct);
        
        if (setting != null)
        {
            //update
            setting.Metadata = JsonSerializer.Serialize(value);
            await repository.UpdateAsync(setting, ct);
        }
        else
        {
            //insert
            var metadata = JsonSerializer.Serialize(value);
            setting = new Setting {
                Name = key.ToLowerInvariant(),
                Metadata = metadata,
                TenantName = tenantName.ToLower()
            };
            await repository.AddAsync(setting, ct);
        }
        
        await repository.SaveChangesAsync(ct);
    }

    public Task SaveSettingAsync<T>(string key, T value, string tenantName = "", CancellationToken ct = default) where T : ISettings, new()
    {
        return Task.CompletedTask;
    }
    
    public async Task SaveSettingAsync<T>(T value, string tenantName = "", CancellationToken ct = default) where T : ISettings, new()
    {
        var query = SettingsByNameQuery(typeof(T).Name);
        query = SettingsByTenantQuery(query, tenantName);
        
        var setting = await query.FirstOrDefaultAsync(ct);
        
        if (setting != null)
        {
            //update
            setting.Metadata = JsonSerializer.Serialize(value);
            await repository.UpdateAsync(setting, ct);
        }
        else
        {
            //insert
            var metadata = JsonSerializer.Serialize(value);
            setting = new Setting {
                Name = typeof(T).Name.ToLowerInvariant(),
                Metadata = metadata,
                TenantName = tenantName.ToLower()
            };
            await repository.AddAsync(setting, ct);
        }

        await repository.SaveChangesAsync(ct);
    }

    public virtual ISettings? LoadSetting(Type type,string tenantName = "")
    {
        var query =  SettingsByNameQuery(type.Name);
        query = SettingsByTenantQuery(query, tenantName);
        var setting = query.FirstOrDefault();

        return TrySerializeSettings(type, setting);
    }

    public async Task<ISettings?> LoadSettingAsync(Type type, string? tenantName = "", CancellationToken ct = default)
    {
        IQueryable<Setting> query = Repository.Queryable().AsNoTracking();
        
        query = SettingsByTenantQuery(query, tenantName);
      
        var name = type.Name.ToLowerInvariant();
        query = query.Where(x => x.Name == name);
        
        var setting = await query.FirstOrDefaultAsync(ct);
        
        return TrySerializeSettings(type, setting);
    }

    public virtual Task<T?> LoadSettingAsync<T>(string tenantName = "", CancellationToken ct = default) where T : ISettings, new()
    {
         return Task.FromResult((T)LoadSetting(typeof(T), tenantName));
    }

    public virtual async Task<T?> GetSettingByKeyAsync<T>(string key, T? defaultValue = default, string tenantName = "", CancellationToken ct = default)
    {
        if (key.IsNullOrEmpty()) return defaultValue;
        
        var query =  SettingsByNameQuery(key);
        query = SettingsByTenantQuery(query, tenantName);
        
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

    public async Task DeleteSetting<T>(string tenantName = "", CancellationToken ct = default) where T : ISettings, new()
    {
        var query = SettingsByNameQuery(typeof(T).Name);
        query = SettingsByTenantQuery(query, tenantName);

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
        
        name = name.ToLowerInvariant();
        return Repository.Queryable().Where(x => x.Name == name);
    }
    
    private IQueryable<Setting> SettingsByTenantQuery(IQueryable<Setting> query, string tenantName = "")
    {
        if (!tenantName.IsNullOrEmpty())
        {
            tenantName = tenantName.ToLower();
            query = query.Where(x => x.TenantName == tenantName);
        }
        
        return query;
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