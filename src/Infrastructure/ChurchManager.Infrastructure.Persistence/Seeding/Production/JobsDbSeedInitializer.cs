#region

using ChurchManager.Domain.Features.Groups.Jobs;
using ChurchManager.Infrastructure.Abstractions.Configuration;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using CodeBoss.Extensions;
using CodeBoss.Jobs.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Seeding.Production;

public class JobsDbSeedInitializer(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = 99;
    
    public async Task InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();
        var jobsOptions = scope.ServiceProvider.GetRequiredService<IOptions<JobsOptions>>().Value;
        
        // Run every day at 8 AM or every 30 seconds when debugging is enabled
        var cronExpression = jobsOptions.DebugEnabled? "0/30 * * * *?" : "0 0 8 * *?"; 
        
        if (await dbContext.ServiceJobs.CountAsync() <= 2)
        {
            var job = new ServiceJob
            {
                Name = nameof(NotifyWorstGroupMemberAttendanceJob).SplitCase(),
                JobKey = Guid.NewGuid(),
                Description = "Determine the worst attendees in a group and send a notification.",
                CronExpression = cronExpression,
                IsActive = true,
                JobParameters = new Dictionary<string, string>{ { "groupId", "2" } },
                Assembly = typeof(NotifyWorstGroupMemberAttendanceJob).Assembly.FullName,
                Class = typeof(NotifyWorstGroupMemberAttendanceJob).FullName
            };
          
            await dbContext.ServiceJobs.AddAsync(job);
            await dbContext.SaveChangesAsync();
            
            Console.WriteLine($"Added job: [{job.Name}]");
        }
    }
    
}