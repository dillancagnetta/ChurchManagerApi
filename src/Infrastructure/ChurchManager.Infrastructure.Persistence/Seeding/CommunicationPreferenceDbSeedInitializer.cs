using ChurchManager.Domain.Features.Communications;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding;

public class CommunicationPreferenceDbSeedInitializer(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = 99;
    
    private record PreferenceData(string Description, bool CanOverride, string CommunicationType);
    
    public async Task InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();
        var faker = new Bogus.Faker();
        List<string> communicationTypes = [CommunicationType.Email.Value, CommunicationType.SMS.Value];

        if (!await dbContext.CommunicationPreferenceType.AnyAsync())
        {
            var communicationPreferences = new Dictionary<string, PreferenceData>
            {
                { "Church Services", new PreferenceData("Regular church service notifications", true, CommunicationType.SMS.Value) },
                { "Church Special Events", new PreferenceData("Special church events and activities", true, CommunicationType.SMS.Value) },
                { "Church Group Special Events", new PreferenceData("Special church group events and activities", true, CommunicationType.SMS.Value) },
                { "Zonal Special Events", new PreferenceData("Special zonal events and activities", true, CommunicationType.SMS.Value) },
                { "Announcements", new PreferenceData("General church announcements", true, CommunicationType.Email.Value) },
                { "Volunteer Opportunities", new PreferenceData("Volunteer and ministry opportunities", true, CommunicationType.Email.Value) }
            };

            foreach (var preference in communicationPreferences)
            {
                dbContext.CommunicationPreferenceType.Add(
                    new()
                    {
                        Name = preference.Key,
                        Description = preference.Value.Description,
                        IsSystem = true,
                        DefaultNotSetValue = true,
                        CanOverride = preference.Value.CanOverride,
                        DefaultCommunicationType = preference.Value.CommunicationType
                    }
                );
            }

            await dbContext.SaveChangesAsync();
        }
        
        if (!await dbContext.CommunicationPreference.AnyAsync())
        {
            var people = await dbContext.Person.AsNoTracking().Take(10).ToListAsync();
            var preferenceTypes = await dbContext.CommunicationPreferenceType.AsNoTracking().ToListAsync();
            
            // We insert to preferences for SMS and email for the first 10 people
            foreach (var person in people)
            {
                dbContext.CommunicationPreference.Add(
                    new()
                    {
                        CommunicationType = faker.PickRandom(communicationTypes),
                        PersonId = person.Id,
                        PreferenceTypeId = faker.PickRandom(preferenceTypes).Id,
                        IsEnabled = true
                    }
                );
                
                /*
                dbContext.CommunicationPreference.Add(
                    new()
                    {
                        CommunicationType = CommunicationType.Email.Value,
                        PersonId = person.Id,
                        PreferenceTypeId = faker.PickRandom(preferenceTypes).Id,
                        IsEnabled = true
                    }
                );*/
            }

            await dbContext.SaveChangesAsync();
        }
    }
}