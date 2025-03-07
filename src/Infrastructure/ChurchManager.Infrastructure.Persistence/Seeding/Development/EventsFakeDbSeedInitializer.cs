#region

using Bogus;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.People;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using CodeBoss.Extensions;
using Ical.Net;
using Ical.Net.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development;

public class EventsFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = 99;

    public async Task InitializeAsync()
    {
        var calendarSerializer = new CalendarSerializer();
        var faker = new Faker("en");
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

        // Event Types
        if (!await dbContext.EventType.AnyAsync())
        {
            var eventTypes = new Faker<EventType>()
                .RuleFor(e => e.Name,
                    f => f.PickRandom("Super Sunday", "ReachOut World", "Youth Conference", "Prayer Conference",
                        "Worship Night", "Community Outreach"))
                .RuleFor(e => e.Description, f => f.Lorem.Sentence())
                .RuleFor(e => e.IconCssClass,
                    f => f.PickRandom("fa-church", "fa-book-bible", "fa-users", "fa-hands-praying", "fa-music",
                        "fa-hands-helping"))
                .RuleFor(e => e.OnlineSupport,
                    f => f.PickRandom(OnlineSupport.NotOnline, OnlineSupport.Both, OnlineSupport.OnlineOnly))
                .RuleFor(e => e.AgeClassification,
                    f => f.PickRandom(AgeClassification.Adult, AgeClassification.Child, AgeClassification.Teen))
                .RuleFor(e => e.RequiresRegistration, f => faker.Random.Bool(0.8f))
                .RuleFor(e => e.AllowFamilyRegistration, f => faker.Random.Bool(0.8f))
                .RuleFor(e => e.AllowNonFamilyRegistration, f => faker.Random.Bool(0.8f))
                .RuleFor(e => e.RequiresChildInfo, f => faker.Random.Bool(0.3f))
                .RuleFor(e => e.DefaultGroupTypeId, f => 2)
                .Generate(4);

            await dbContext.EventType.AddRangeAsync(eventTypes);
            await dbContext.SaveChangesAsync();
        }

        // Events
        if (!await dbContext.Event.AnyAsync())
        {
            var eventTypes = await dbContext.EventType.ToListAsync();
            var churches = await dbContext.Church.ToListAsync();
            var people = await dbContext.Person.Take(10).ToListAsync();
            var eventRegistrationGroup =
                await dbContext.Group.FirstOrDefaultAsync(g => g.Name == "Event Registration Group");

            var events = new Faker<Event>()
                .RuleFor(e => e.Name, f => string.Join(" ", f.Lorem.Words(3)))
                .RuleFor(e => e.Description, f => f.Lorem.Paragraph())
                .RuleFor(e => e.EventTypeId, f => f.PickRandom(eventTypes).Id)
                .RuleFor(e => e.ChurchId, f => f.PickRandom(churches).Id)
                .RuleFor(e => e.ContactPersonId, f => f.PickRandom(people).Id)
                .RuleFor(e => e.ContactEmail, f => f.Person.Email)
                .RuleFor(s => s.Location, f => f.Address.FullAddress())
                .RuleFor(e => e.ContactPhone, f => f.Person.Phone)
                .RuleFor(e => e.ApprovalStatus, f => ApprovalStatus.Approved)
                .RuleFor(e => e.EventRegistrationGroup, f => eventRegistrationGroup)
                .RuleFor(e => e.Capacity, f => f.Random.Number(50, 500))
                .RuleFor(e => e.PhotoUrl, f => f.Image.PicsumUrl())
                //.RuleFor(e => e.HasChildCare, f => f.Random.Bool())
                //.RuleFor(e => e.MinChildAge, (f, e) => e.HasChildCare ? f.Random.Number(0, 5) : null)
                //.RuleFor(e => e.MaxChildAge, (f, e) => e.HasChildCare ? f.Random.Number((int)e.MinChildAge + 1, 12) : null)
                .Generate(10);
            
            // Generate event sessions for each event
            foreach (var evt in events)
            {
                var sessionDate = DateTime.Now;
                var sessions = new Faker<EventSession>()
                    .RuleFor(s => s.EventId, evt.Id)
                    .RuleFor(s => s.Name, f => f.Lorem.Word())
                    .RuleFor(s => s.EndDateTime, sessionDate.AddHours(faker.Random.Number(1, 5)))
                    .RuleFor(s => s.StartDateTime, sessionDate)
                    .RuleFor(s => s.Description, f => f.Lorem.Sentence())
                    .RuleFor(s => s.Location, f => f.Address.FullAddress())
                    .RuleFor(e => e.AttendanceRequired, f => faker.Random.Bool(0.3f))
                    .RuleFor(e => e.SessionOrder, f => f.Random.Number(0, 4))
                    .RuleFor(e => e.OnlineMeetingUrl, f => f.Internet.Url())
                    .RuleFor(e => e.OnlineSupport,
                        f => f.PickRandom(OnlineSupport.NotOnline, OnlineSupport.Both, OnlineSupport.OnlineOnly))
                    .Generate(faker.Random.Number(1, 5));

                evt.Sessions = sessions.OrderBy(x => x.StartDateTime).ToList();

                var startDateTime = evt.Sessions.First().StartDateTime;
                var endDateTime = evt.Sessions.Last().EndDateTime;

                var calendar = InetCalendarHelper.CreateCalendarWithRecurrence(
                    FrequencyType.Daily,
                    startDateTime.Value,
                    durationMinutes: faker.Random.Number(60, 240),
                    endDateTime,
                    timezone: "South Africa Standard Time");
                
                evt.Schedule = new Schedule
                {
                    Name = $"{evt.Name}-Schedule",
                    StartDate = startDateTime,
                    EndDate = endDateTime,
                    StartTime = startDateTime.Value.TimeOfDay,
                    EndTime = endDateTime.Value.TimeOfDay,
                    iCalendarContent = calendarSerializer.SerializeToString(calendar),
                    Frequency = FrequencyType.Daily.ConvertToString(),
                    Timezone = "South Africa Standard Time"
                };
            }

            await dbContext.Event.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();
        }
    }
}