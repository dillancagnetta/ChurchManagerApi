#region

using ChurchManager.Domain.Features.Finances;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Seeding;

public class FinancesDbSeedInitialize(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = 99;
    private record PartnershipData(string Description, string Code, string? ParentName = null);
    private record FundData(string Description, string Code, FundType Type, string? PartnershipName = null);
    
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
        if (!dbContext.Partnership.Any())
        {
            var partnerships = new Dictionary<string, PartnershipData>
            {
                { HealingSchoolName, new PartnershipData("Healing School Partnership", "HS") },
                { "Healing School Magazine", new PartnershipData("Healing School Magazine", "HTTNM", ParentName:HealingSchoolName) },
                { "Rhapsody of Realities", new PartnershipData("Rhapsody of Realities", "ROR") },
                { ReachOutName, new PartnershipData("Reach out campaigns with Rhapsody of Realities", ReachOutName, ParentName:"Rhapsody of Realities") },
                { "Inner City", new PartnershipData("Inner City Missions", "ICM") },
            };

            var inserted = new Dictionary<string, Partnership>();
            // Step 1:Insert Roots
            foreach (var kvp in partnerships.Where(x => x.Value.ParentName == null))
            {
                var entity = new Partnership
                {
                    Name = kvp.Key,
                    Description = kvp.Value.Description,
                    Code = kvp.Value.Code,
                };

                dbContext.Partnership.Add(entity);
                
                inserted[kvp.Key] = entity;
            }
            
            // Save to generate IDs for parent records
            await dbContext.SaveChangesAsync();
            
            // Step 2: Insert child partnerships with proper ParentId
            foreach (var kvp in partnerships.Where(x => x.Value.ParentName != null))
            {
                var parent = inserted[kvp.Value.ParentName!];

                var entity = new Partnership
                {
                    Name = kvp.Key,
                    Description = kvp.Value.Description,
                    Code = kvp.Value.Code,
                    ParentPartnershipId = parent.Id
                };

                dbContext.Partnership.Add(entity);
            }

            await dbContext.SaveChangesAsync();
        }
    }
    
    private static async Task SeedFundsAsync(ChurchManagerDbContext dbContext)
    {
        if (!dbContext.Fund.Any())
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
            
            await dbContext.SaveChangesAsync();
        }
    }
}