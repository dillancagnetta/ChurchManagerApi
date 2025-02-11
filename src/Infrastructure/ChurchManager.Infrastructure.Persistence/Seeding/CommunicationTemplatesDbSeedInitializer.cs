#region

using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Seeding;

public class CommunicationTemplatesDbSeedInitializer(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = 99;
    
    public async Task InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

        if (!await dbContext.CommunicationTemplate.AnyAsync())
        {
            var template1 = await CreateCommunicationTemplate(DomainConstants.Communication.Email.Templates.FollowUpTemplate);
            var template2 = await CreateCommunicationTemplate(DomainConstants.Communication.Email.Templates.FamilyCodeRequest);
            var baseTemplate = await CreateCommunicationTemplate(DomainConstants.Communication.Email.Templates.Layout, true);
            
            await dbContext.CommunicationTemplate.AddRangeAsync(template1, template2, baseTemplate);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<CommunicationTemplate> CreateCommunicationTemplate(string name, bool isBaseTemplate = false)
    {
        var path = DomainConstants.Communication.Email.Template(name);
        string content = await File.ReadAllTextAsync(path);

        return new CommunicationTemplate
        {
            Name = name.Replace("_", ""), // Removes leading underscore
            Description = name.SplitCase(),
            Content = content,
            IsBaseTemplate = isBaseTemplate,
            IsSystem = true,
            LogoFileUrl = isBaseTemplate ? "https://churchmanager-assets.s3.us-east-1.amazonaws.com/logo.svg" : null
        };
    }
}