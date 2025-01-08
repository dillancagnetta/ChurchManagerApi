using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using CodeBoss.Jobs.Model;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development;

public class JobsFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = 99;
    
    public async Task InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

        if (!dbContext.ServiceJobs.Any())
        {
            var job = new ServiceJob
            {
                Name = "Test Job One",
                JobKey = Guid.NewGuid(),
                Description = "This is a test job.",
                CronExpression = "0/10 * * * * ?", // every 10 seconds
                IsActive = true,
                JobParameters = new Dictionary<string, string>() { { "groupId", "3" } },
                Assembly = "ChurchManager.Infrastructure.Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                Class = "ChurchManager.Infrastructure.Shared.Jobs.TestJob"
            };
            
            var job2 = new ServiceJob
            {
                Name = "Test Job Two",
                JobKey = Guid.NewGuid(),
                Description = "This is a test job.",
                CronExpression = "0/10 * * * * ?", // every 10 seconds
                IsActive = true,
                EnableHistory = true,
                JobParameters = null,
                Assembly = "ChurchManager.Infrastructure.Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                Class = "ChurchManager.Infrastructure.Shared.Jobs.TestJobTwo"
            };
            
            await dbContext.ServiceJobs.AddAsync(job);
            await dbContext.ServiceJobs.AddAsync(job2);
            await dbContext.SaveChangesAsync();
        }
    }
    
}