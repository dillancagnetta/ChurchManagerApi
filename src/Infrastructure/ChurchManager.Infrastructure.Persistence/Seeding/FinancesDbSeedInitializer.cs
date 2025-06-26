#region

using ChurchManager.Domain.Features.Finances;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Seeding;

public class FinancesDbSeedInitialize(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = 105;

    private record Data(string Description, string Code, string Category, string? ParentName = null);
    
    private const string HealingSchoolName = "Healing School";
    private const string ReachOutName = "ReachOut";

    public async Task InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

        await SeedPartnershipsAsync(dbContext);
    }

    private static async Task SeedPartnershipsAsync(ChurchManagerDbContext dbContext)
    {
        if (!dbContext.Fund.Any())
        {
            dbContext.Fund.Add(new Fund { Name = "Default", Code = "DEFAULT", FundType = FundType.Unknown, IsSystem = true, Description = "Default fund"});
            
            var funds = new Dictionary<string, Data>
            {
                { FundType.Partnership.Value, new Data("Partnership Main Section", "PARTNER", "Partnership") },
                {
                    HealingSchoolName,
                    new Data("Healing School Partnership", "HS", "Partnership",
                        ParentName: FundType.Partnership.Value)
                },
                {
                    "Healing School Magazine",
                    new Data("Healing School Magazine", "HTTM", "Partnership",
                        ParentName: HealingSchoolName)
                },
                {
                    "Rhapsody of Realities",
                    new Data("Rhapsody of Realities", "ROR", "Partnership",
                        ParentName: FundType.Partnership.Value)
                },
                {
                    ReachOutName,
                    new Data("Reach out campaigns with Rhapsody of Realities", ReachOutName, "Partnership",
                        ParentName: "Rhapsody of Realities")
                },
                {
                    "Inner City",
                    new Data("Inner City Missions", "ICM", "Partnership",
                        ParentName: FundType.Partnership.Value)
                },

                { FundType.General.Value, new Data("General Main Section", "GENERAL", "General") },
                {
                    "Tithes",
                    new Data("Regular tithes giving", "T", FundType.General.Value,
                        ParentName: FundType.General.Value)
                },
                {
                    "Offerings",
                    new Data("General offerings", "O", FundType.General.Value,
                        ParentName: FundType.General.Value)
                },
                {
                    "First Fruits",
                    new Data("First fruits offerings", "FF", FundType.General.Value,
                        ParentName: FundType.General.Value)
                }
            };

            // Track inserted for ids
            var inserted = new Dictionary<string, Fund>();
            
            // Step 1:Insert Roots
            var rootKvps = funds.Where(x => x.Value.ParentName == null).ToList();
            foreach (var kvp in rootKvps)
            {
                var entity = new Fund
                {
                    Name = kvp.Key,
                    Description = kvp.Value.Description,
                    Code = kvp.Value.Code,
                    FundType = kvp.Value.Category
                };

                dbContext.Fund.Add(entity);
                inserted[kvp.Key] = entity;
            }

            // Save to generate IDs for parent records
            await dbContext.SaveChangesAsync();
            
            /*------------------------------------------*/

            /* recursively add child funds */
            while (rootKvps.Any())
            {
                var parentNames = rootKvps.Select(x => x.Key).ToList();
                // Step 2: Insert child partnerships with proper ParentId
                var childKvps = funds.Where(x =>
                    x.Value.ParentName != null && parentNames.Contains(x.Value.ParentName)).ToList();
                foreach (var kvp in childKvps)
                {
                    var parent = inserted[kvp.Value.ParentName!];

                    var entity = new Fund
                    {
                        Name = kvp.Key,
                        Description = kvp.Value.Description,
                        Code = kvp.Value.Code,
                        FundType = kvp.Value.Category,
                        ParentFundId = parent.Id
                    };

                    dbContext.Fund.Add(entity);
                    inserted[kvp.Key] = entity;
                }

                await dbContext.SaveChangesAsync();
                // reset 
                rootKvps = childKvps;
            }
        }
    }

    private static Task SeedFundsAsync(ChurchManagerDbContext dbContext)
    {
        /*if (!dbContext.Fund.Any())
        {
            var funds = new Dictionary<string, FundData>
            {
                { "Tithes", new FundData("Regular tithes giving", "GEN-TITHE",  FundType.General) },
                { "General Offering", new FundData("General offerings", "GEN-OFFER",  FundType.General) },
                { "First Fruits", new FundData("First fruits offerings", "GEN-FIRST",  FundType.General) },
                { "Building", new FundData("Building & Maintenance", "PRO-BUILD",  FundType.ChurchProjects) },
                // Partnership funds
                { HealingSchoolName, new FundData("Healing School General", "PAR-HS",  FundType.Partnership, PartnershipName:HealingSchoolName) },
                { ReachOutName, new FundData("ReachOut General", "PAR-RO",  FundType.Partnership, PartnershipName:ReachOutName) },
            };

            foreach (var kvp in funds)
            {
                var entity = new Fund
                {
                    Name = kvp.Key,
                    Description = kvp.Value.Description,
                    Code = kvp.Value.Code,
                    Type = kvp.Value.Type,
                    PartnershipId = dbContext.Partnership.FirstOrDefault(x => x.Name == kvp.Value.PartnershipName)?.Id,
                };

                dbContext.Fund.Add(entity);
            }

            await dbContext.SaveChangesAsync();*/

        return Task.CompletedTask;
    }
}