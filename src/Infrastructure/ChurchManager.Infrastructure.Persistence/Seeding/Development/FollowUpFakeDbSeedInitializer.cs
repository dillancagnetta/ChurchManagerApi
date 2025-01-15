#region

using Bogus;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Person = ChurchManager.Domain.Features.People.Person;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development
{
    public class FollowUpFakeDbSeedInitializer : IInitializer
    {
        public int OrderNumber => 80;

        private readonly IServiceScopeFactory _scopeFactory;    

        private string[] Severity => new[] { "Normal", "Urgent" };
        private string[] Types => new[] { "New Convert", "General Well Being", "Home Visitation", "Death" };

        public FollowUpFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await dbContext.FollowUp.AnyAsync())
            {
                var people = dbContext.Person.AsQueryable().AsNoTracking().Take(100);

                var faker = new Faker();
                var random = new Random();

                var items = new List<FollowUp>(400);

                foreach(var person in people)
                {
                    var count = random.Next(1, 4);
                    for (int i = 0; i < count; i++)
                    {
                        items.AddRange(FollowUpRecordsForPerson(person, faker, random));
                    }
                }

                await dbContext.FollowUp.AddRangeAsync(items);
                await dbContext.SaveChangesAsync();
            }
        }
        
        public IEnumerable<FollowUp> FollowUpRecordsForPerson(Person person, Faker faker, Random random)
        {
            yield return new FollowUp
            {
                Type = faker.PickRandom(Types),
                Severity = faker.PickRandom(Severity),
                AssignedPersonId = random.Next(1, 4),
                PersonId = person.Id,
                AssignedDate = faker.Date.Between(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 01), DateTime.UtcNow),
                RequiresAdditionalFollowUp = faker.PickRandom(true, false),
                RecordStatus = RecordStatus.Active,
                ActionDate = null,
            };
        }
    }
}
