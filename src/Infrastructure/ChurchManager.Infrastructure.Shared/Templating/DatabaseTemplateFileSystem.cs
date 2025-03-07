using System.Text.Json;
using ChurchManager.Domain.Features.Communications.Repositories;
using CodeBoss.Extensions;
using DotLiquid;
using DotLiquid.FileSystems;
using Microsoft.Extensions.Caching.Distributed;

namespace ChurchManager.Infrastructure.Shared.Templating;

public class DatabaseTemplateFileSystem(
    ITemplateDbRepository templateDb,
    IDistributedCache cache) : IFileSystem
{   
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

    public string ReadTemplateFile(Context context, string templateName)
    {
        // Remove any file extensions if present
        templateName = templateName.Replace(".liquid", "").Replace(".html", "");
        
        var cacheKey = $"template_{templateName}";
        
        // Fetch from cache if available
        var data = cache.GetString(cacheKey);
        if (!data.IsNullOrWhiteSpace()) return data;
        
        // Fetch template from database
        var template = templateDb.TemplateByNameAsync(templateName, default).GetAwaiter().GetResult();
        
        if (template == null)
            throw new FileNotFoundException($"Template '{templateName}' not found in database.");
            
        // Save in cache
        var serializedTemplate = JsonSerializer.Serialize(template.Content);
        var cacheEntryOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(_cacheDuration);
        cache.SetString(cacheKey, serializedTemplate, cacheEntryOptions);
        
        return template.Content;
    }
}