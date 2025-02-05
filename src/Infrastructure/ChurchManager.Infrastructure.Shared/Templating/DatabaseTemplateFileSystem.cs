using ChurchManager.Domain.Features.Communication.Repositories;
using DotLiquid;
using DotLiquid.FileSystems;

namespace ChurchManager.Infrastructure.Shared.Templating;

public class DatabaseTemplateFileSystem(ITemplateRepository templateDb) : IFileSystem
{
    
    public string ReadTemplateFile(Context context, string templateName)
    {
        // Remove any file extensions if present
        templateName = templateName.Replace(".liquid", "").Replace(".html", "");
        
        // Fetch template from database
        var template = templateDb.TemplateByNameAsync(templateName, default).GetAwaiter().GetResult();
        
        if (template == null)
            throw new FileNotFoundException($"Template '{templateName}' not found in database.");
            
        return template.Content;
    }
}