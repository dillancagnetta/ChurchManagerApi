using ChurchManager.Domain.Features;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development;

public class MessagesFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = 99;
    
    public async Task InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

        if (!await dbContext.Message.AnyAsync())
        {
            var userLoginId = Guid.Parse(SeedingConstants.MainUserLogin);
            var sampleMessages = GenerateSampleMessages(userLoginId);
            
            await dbContext.Message.AddRangeAsync(sampleMessages);
            await dbContext.SaveChangesAsync();
        }
    }

    private List<Message> GenerateSampleMessages(Guid userLoginId)
    {
        return new List<Message>
        {
            new Message
            {
                Title = "Welcome to Our Church",
                Body = "We're delighted to have you join our community. Please feel free to reach out if you have any questions!",
                SentDateTime = DateTime.Now.AddDays(-7),
                UserId = userLoginId, 
                IconCssClass = "heroicons_outline:folder",
                Classification = MessageClassification.Info,
                UseRouter = false,
                Status = MessageStatus.Sent // Testing, prevents being sent to the user
            },
            new Message
            {
                Title = "Upcoming Event: Summer Picnic",
                Body = "Don't forget about our annual summer picnic this Saturday at 2 PM in the church garden. Bring your favorite dish!",
                SentDateTime = DateTime.Now.AddDays(-3),
                UserId = userLoginId, 
                IconCssClass = "heroicons_outline:squares-2x2",
                Classification = MessageClassification.Info,
                UseRouter = false,
                Status = MessageStatus.Sent // Testing, prevents being sent to the user
            },
            new Message
            {
                Title = "Prayer Request",
                Body = "Please keep Sarah in your prayers as she undergoes surgery next week.",
                SentDateTime = DateTime.Now.AddDays(-1),
                UserId = userLoginId, 
                IconCssClass = "heroicons_outline:folder",
                Classification = MessageClassification.Warning,
                UseRouter = false,
                Status = MessageStatus.Sent // Testing, prevents being sent to the user
            },
            new Message
            {
                Title = "Volunteer Opportunity",
                Body = "We need volunteers for our upcoming community outreach program. If you're interested, please reply to this message.",
                SentDateTime = DateTime.Now,
                UserId = userLoginId, 
                IconCssClass = "heroicons_outline:squares-2x2",
                Classification = MessageClassification.Info,
                UseRouter = false,
                Status = MessageStatus.Sent // Testing, prevents being sent to the user
            },
            new Message
            {
                Title = "Thank You for leading the Group",
                Body = "We want to express our gratitude for your dedication to the church group.",
                SentDateTime = DateTime.Now.AddHours(-12),
                UserId = userLoginId, 
                IconCssClass = "heroicons_outline:folder",
                Classification = MessageClassification.Info,
                UseRouter = true,
                Link = "/groups/2",
                Status = MessageStatus.Sent // Testing, prevents being sent to the user
            }
        };
    }
}