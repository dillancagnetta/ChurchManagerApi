#region

using Bogus;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development;

/// <summary>
/// Seeds the database with some dummy data
/// </summary>
public class ChurchesFakeDbSeedInitializer : IInitializer
{
    public int OrderNumber { get; } = 0;
    private readonly IServiceScopeFactory _scopeFactory;

    public ChurchesFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

    public async Task InitializeAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

        var southAfricaProvinces = new Dictionary<string, List<string>>
        {
            { "Western Cape", new List<string> 
                { "Cape Town", "Stellenbosch", "Paarl", "George", "Mossel Bay", 
                    "Worcester", "Somerset West", "Hermanus", "Knysna", "Beaufort West" } 
            },
            { "Gauteng", new List<string> 
                { "Johannesburg", "Pretoria", "Sandton", "Soweto", "Midrand", 
                    "Centurion", "Benoni", "Boksburg", "Alberton", "Kempton Park" } 
            }
        };
            
        if(!await dbContext.Church.AnyAsync())
        {
            var faker = new Faker();

            foreach (var province in southAfricaProvinces)
            {
                var churchGroup = new ChurchGroup { Name = province.Key + " Group", Description = province.Key + " Church Group" };
                foreach (var city in province.Value)
                {
                    dbContext.Church.Add(new Church
                    {
                        Name = city + " Church",
                        Description = city + " Church",
                        ShortCode = faker.Address.ZipCode(),
                        PhoneNumber = faker.Phone.PhoneNumber(),
                        ChurchGroup = churchGroup,
                    });
                }
            }
                
            await dbContext.SaveChangesAsync();
        }
    }
}