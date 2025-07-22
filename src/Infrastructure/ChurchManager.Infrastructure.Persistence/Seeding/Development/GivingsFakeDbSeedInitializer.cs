#region

using Bogus;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development;

public class GivingsFakeDbSeedInitializer : IInitializer
{
    public int OrderNumber => 110;

    private readonly IServiceScopeFactory _scopeFactory;
    
    private GivingType[] OftenGivingType => new[] { GivingType.Seed, GivingType.Offering, GivingType.Tithe };
    private GivingType[] RareGivingType => new[] { GivingType.FirstFruit , GivingType.Tithe};

    public GivingsFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

    public async Task InitializeAsync()
    {
        var faker = new Faker();
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

        if (!await dbContext.Benefactor.AnyAsync())
        {
            var people = dbContext.Person.AsNoTracking().Take(10).ToList();
            var families = dbContext.Family.AsNoTracking().Take(3).ToList();
            var groups = dbContext.Group.AsNoTracking().Take(3).ToList();
            var churches = dbContext.Church.AsNoTracking().Take(3).ToList();

            var peopleBenefactors = people.Select(Benefactor.FromPerson).ToList();
            var familiesBenefactors = families.Select(Benefactor.FromFamily).ToList();
            var groupsBenefactors = groups.Select(Benefactor.FromGroup).ToList();
            var churchesBenefactors = churches.Select(Benefactor.FromChurch).ToList();
                
            await dbContext.Benefactor.AddRangeAsync(peopleBenefactors);
            await dbContext.Benefactor.AddRangeAsync(familiesBenefactors);
            await dbContext.Benefactor.AddRangeAsync(churchesBenefactors);
            await dbContext.Benefactor.AddRangeAsync(groupsBenefactors);
            await dbContext.SaveChangesAsync();

            if (!await dbContext.Giving.AnyAsync())
            {
                var funds = dbContext.Fund.AsNoTracking().ToList();
                var generalFunds = funds.Where(x => x.FundType.Value == FundType.General.Value)
                    .Select(x =>x.Id).ToList();
                var partnershipFunds = funds.Where(x => x.FundType.Value == FundType.Partnership.Value)
                    .Select(x =>x.Id).ToList();
                var givings = new List<Giving>();

                for (int i = 0; i < 100; i++)
                {
                    // People
                    var personBenefactor = faker.PickRandom(peopleBenefactors);
                    givings.Add(new Giving
                    {
                        Date = faker.Date.Between(DateTime.Today.AddMonths(-6), DateTime.Today),
                        GivingAmount = new Money(Currency.ZAR,  faker.Finance.Amount(100M, 50000M)),
                        GivingType = faker.PickRandom(OftenGivingType),
                        Benefactor = personBenefactor,
                        PaymentMethod = PaymentMethod.EFT,
                        FundId = faker.PickRandom(generalFunds),
                        ChurchId = people.First(x => x.Id == personBenefactor.PersonId).ChurchId,
                        BankTransactionId = faker.Finance.RoutingNumber(),
                        ParsedReference = faker.Finance.RoutingNumber(),
                    });
                        
                    personBenefactor = faker.PickRandom(peopleBenefactors);
                    givings.Add(new Giving
                    {
                        Date = faker.Date.Between(DateTime.Today.AddMonths(-6), DateTime.Today),
                        GivingAmount = new Money(Currency.ZAR,  faker.Finance.Amount(100M, 50000M)),
                        GivingType = GivingType.Partnership,
                        Benefactor = personBenefactor,
                        PaymentMethod = PaymentMethod.EFT,
                        FundId = faker.PickRandom(partnershipFunds),
                        ChurchId = people.First(x => x.Id == personBenefactor.PersonId).ChurchId,
                        BankTransactionId = faker.Finance.RoutingNumber(),
                        ParsedReference = faker.Finance.RoutingNumber(),
                    });
                        
                    // Families
                    givings.Add(new Giving
                    {
                        Date = faker.Date.Between(DateTime.Today.AddMonths(-6), DateTime.Today),
                        GivingAmount = new Money(Currency.ZAR,  faker.Finance.Amount(100M, 50000M)),
                        GivingType = GivingType.Partnership,
                        Benefactor = faker.PickRandom(familiesBenefactors),
                        PaymentMethod = PaymentMethod.EFT,
                        FundId = faker.PickRandom(partnershipFunds),
                        ChurchId = faker.PickRandom(churches).Id,
                        BankTransactionId = faker.Finance.RoutingNumber(),
                        ParsedReference = faker.Finance.RoutingNumber(),
                    });
                }

                // Churches
                for (int i = 0; i < 50; i++)
                {
                    var churchesBenefactor = faker.PickRandom(churchesBenefactors);
                    givings.Add(new Giving
                    {
                        Date = faker.Date.Between(DateTime.Today.AddMonths(-6), DateTime.Today),
                        GivingAmount = new Money(Currency.ZAR,  faker.Finance.Amount(1000M, 500000M)),
                        GivingType = faker.PickRandom(RareGivingType),
                        Benefactor = churchesBenefactor,
                        PaymentMethod = PaymentMethod.EFT,
                        FundId = faker.PickRandom(generalFunds),
                        ChurchId = churches.First(x => x.Id == churchesBenefactor.ChurchId).Id,
                        BankTransactionId = faker.Finance.RoutingNumber(),
                        ParsedReference = faker.Finance.RoutingNumber(),
                    }); 
                }
                
                await dbContext.Giving.AddRangeAsync(givings);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}