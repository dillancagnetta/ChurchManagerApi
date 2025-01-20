using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding;


public class ConnectionTypesDbSeedInitializer(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = SeedingConstants.FirstSeedOrder;
    
    public async Task InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

        if (!await dbContext.ConnectionStatusType.AnyAsync())
        {
            var connectionStatuses = new List<ConnectionStatusType>()
            {
                new()
                {
                    
                    Name = ConnectionStatus.Member.ToString(),
                    Description = "A member of the church.",
                    IsSystem = true,
                    RecordStatus = RecordStatus.Active.ToString(),
                    Priority = 1
                },
                new()
                {
                    
                    Name = ConnectionStatus.FirstTimer.ToString(),
                    Description = "A first time visitor.",
                    IsSystem = true,
                    RecordStatus = RecordStatus.Active.ToString(),
                    Priority = 1
                },
                new()
                {
                    
                    Name = ConnectionStatus.NewConvert.ToString(),
                    Description = "A new believer.",
                    IsSystem = true,
                    RecordStatus = RecordStatus.Active.ToString(),
                    Priority = 1
                },
                new()
                {
                    
                    Name = ConnectionStatus.EventRegistrant.ToString(),
                    Description = "Event attendee via registration.",
                    IsSystem = true,
                    RecordStatus = RecordStatus.Active.ToString(),
                    Priority = 1
                },
                new()
                {
                    
                    Name = ConnectionStatus.CellAttendee.ToString(),
                    Description = "Cell attendee but not a member.",
                    IsSystem = true,
                    RecordStatus = RecordStatus.Active.ToString(),
                    Priority = 1
                }
            };
            
            await dbContext.ConnectionStatusType.AddRangeAsync(connectionStatuses);
            await dbContext.SaveChangesAsync();
        }
    }
}