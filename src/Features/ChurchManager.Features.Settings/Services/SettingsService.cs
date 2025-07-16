using System.Text.Json;
using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.Features;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Settings;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.Extensions;
using CodeBoss.MultiTenant;
using Microsoft.EntityFrameworkCore;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Settings.Services;

public class SettingsService(
    IGenericDbRepository<Setting> repository, IMapper mapper, 
    ITenantsProvider<TenantConfiguration> tenantsProvider)
    : CrudServiceAsync<Setting, SettingViewModel, EditSettingViewModel>(repository, mapper), ISettingsService
{
    public virtual async Task SetSettingAsync<T>(string key, T value, int? churchGroupId = null, int? churchId = null, int? personId = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        
        key = key.Trim().ToLowerInvariant();
        var query =  SettingsByNameQuery(key);
        query =  SettingsFilterQuery(query, churchGroupId, churchId, personId);
        
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
                ChurchGroupId = churchGroupId,
                ChurchId = churchId,
                PersonId = personId
            };
            await repository.AddAsync(setting, ct);
        }
        
        await repository.SaveChangesAsync(ct);
    }

    public Task SaveSettingAsync<T>(string key, T value, int? churchGroupId = null, int? churchId = null, int? personId = null,
        CancellationToken ct = default) where T : ISettings, new()
    {
        return Task.CompletedTask;
    }
    
    public async Task SaveSettingAsync<T>(T value, int? churchGroupId = null, int? churchId = null, int? personId = null,
        CancellationToken ct = default) where T : ISettings, new()
    {
        var query = SettingsByNameQuery(typeof(T).Name);
        query =  SettingsFilterQuery(query, churchGroupId, churchId, personId);
        
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
                ChurchGroupId = churchGroupId,
                ChurchId = churchId,
                PersonId = personId
            };
            await repository.AddAsync(setting, ct);
        }

        await repository.SaveChangesAsync(ct);
    }

    public virtual ISettings? LoadSetting(Type type, 
        int? churchGroupId = null, int? churchId = null, int? personId = null)
    {
        var query =  SettingsByNameQuery(type.Name);
        query =  SettingsFilterQuery(query, churchGroupId, churchId, personId);
        var setting = query.FirstOrDefault();

        return TrySerializeSettings(type, setting);
    }

    public async Task<ISettings?> LoadSettingAsync(Type type, string? tenantName, CancellationToken ct = default)
    {
        IQueryable<Setting> query = Repository.Queryable().AsNoTracking();
        
        if (!tenantName.IsNullOrEmpty())
        {
            var tenant = tenantsProvider.Get(tenantName);
            query = SettingsByTenantIdQuery(query, tenant.Id);
        }
      
        var name = type.Name.ToLowerInvariant();
        query = query.Where(x => x.Name == name);
        
        var setting = await query.FirstOrDefaultAsync(ct);
        
        return TrySerializeSettings(type, setting);
    }

    public virtual Task<T?> LoadSettingAsync<T>(int? churchGroupId = null, int? churchId = null, int? personId = null,
        CancellationToken ct = default) where T : ISettings, new()
    {
         return Task.FromResult((T)LoadSetting(typeof(T), churchGroupId, churchId, personId));
    }

    public virtual async Task<T?> GetSettingByKeyAsync<T>(string key, T? defaultValue = default, int? churchGroupId = null, int? churchId = null,
        int? personId = null, CancellationToken ct = default)
    {
        if (key.IsNullOrEmpty()) return defaultValue;
        
        var query =  SettingsByNameQuery(key);
        query =  SettingsFilterQuery(query, churchGroupId, churchId, personId);
        
        var setting = await query.FirstOrDefaultAsync(ct);
    
        return setting != null ? JsonSerializer.Deserialize<T>(setting.Metadata) : defaultValue;
    }

    public Task DeleteSetting<T>() where T : ISettings, new()
    {
        return Task.CompletedTask;
    }

    private IQueryable<Setting> SettingsByNameQuery(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        
        name = name.ToLowerInvariant();
        return Repository.Queryable().Where(x => x.Name == name);
    }
    
    private IQueryable<Setting> SettingsByTenantIdQuery(IQueryable<Setting> query, int tenantId)
    {
        query = query.Where(x => x.TenantId == tenantId);
        return query;
    }
    
    private IQueryable<Setting> SettingsFilterQuery(IQueryable<Setting> query, 
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
    }
    
    private ISettings? TrySerializeSettings(Type type, Setting? setting)
    {
        if (setting == null) return Activator.CreateInstance(type) as ISettings;
        
        var _setting = JsonSerializer.Deserialize(setting.Metadata, type) as ISettings;

        return _setting ?? Activator.CreateInstance(type) as ISettings;
    }
}