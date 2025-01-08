using ChurchManager.Domain.Features.Groups.Jobs;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using CodeBoss.Extensions;
using CodeBoss.Jobs.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development;

public class JobsDbSeedInitializer(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = 99;
    
    public async Task InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

        if (await dbContext.ServiceJobs.CountAsync() <= 2)
        {
            var job = new ServiceJob
            {
                Name = nameof(NotifyWorstGroupMemberAttendanceJob).SplitCase(),
                JobKey = Guid.NewGuid(),
                Description = "Determine the worst attendees in a group and send a notification.",
                CronExpression = "0/30 * * * * ?",
                IsActive = true,
                JobParameters = new Dictionary<string, string>{ { "groupId", "2" } },
                Assembly = typeof(NotifyWorstGroupMemberAttendanceJob).Assembly.FullName,
                Class = typeof(NotifyWorstGroupMemberAttendanceJob).FullName
            };
          
            await dbContext.ServiceJobs.AddAsync(job);
            await dbContext.SaveChangesAsync();
        }
    }
    
}