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
using Person = ChurchManager.Domain.Features.People.Person;

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
            var defaultGroupTypeId = await dbContext.GroupType.Where(x =>  x.Name == "Events").Select(x => x.Id).FirstOrDefaultAsync();
            
            var eventTypes = new Faker<EventType>()
                .RuleFor(e => e.Name,
                    f => f.PickRandom("Super Sunday", "ReachOut World", "Youth Conference", "Prayer Conference",
                        "Worship Night", "Community Outreach"))
                .RuleFor(e => e.Description, f => f.Lorem.Sentence())
                .RuleFor(e => e.IconCssClass,
                    f => f.PickRandom("heroicons_outline:ban", "heroicons_outline:badge-check", "heroicons_outline:book-open", "heroicons_outline:bell", 
                        "heroicons_outline:cake", "heroicons_outline:calendar"))
                .RuleFor(e => e.OnlineSupport,
                    f => f.PickRandom(OnlineSupport.NotOnline, OnlineSupport.Both, OnlineSupport.OnlineOnly))
                .RuleFor(e => e.AgeClassification,
                    f => f.PickRandom(AgeClassification.Adult, AgeClassification.Child, AgeClassification.Teen))
                .RuleFor(e => e.RequiresRegistration, f => faker.Random.Bool(0.8f))
                .RuleFor(e => e.AllowFamilyRegistration, f => faker.Random.Bool(0.8f))
                .RuleFor(e => e.AllowNonFamilyRegistration, f => faker.Random.Bool(0.8f))
                .RuleFor(e => e.RequiresChildInfo, f => faker.Random.Bool(0.3f))
                .RuleFor(e => e.DefaultGroupTypeId, f => defaultGroupTypeId)
                .Generate(4);

            await dbContext.EventType.AddRangeAsync(eventTypes);
            await dbContext.SaveChangesAsync();
        }

        // Events
        if (!await dbContext.Event.AnyAsync())
        {
            var eventTypes = await dbContext.EventType.ToListAsync();
            var churchGroups = await dbContext.ChurchGroup
                .Include(x => x.Churches)
                .Select(x => new ChurchGroupAndChurches
                {
                    ChurchesGroupId = x.Id,
                    ChurchIds = x.Churches.Select(c => c.Id).ToList()
                })
                .ToListAsync();
            var people = await dbContext.Person.Take(10).ToListAsync();
            var eventRegistrationGroup =
                await dbContext.Group.FirstOrDefaultAsync(g => g.Name == "Event Registration Group");

            // Generate Events at different levels
            var zonalEvents = GenerateEvents(eventTypes, people, eventRegistrationGroup, count:2);
            var churchGroup1Events = GenerateEvents(eventTypes, people, eventRegistrationGroup, churchGroup:churchGroups.First(), count:4);
            var churchGroup2Events = GenerateEvents(eventTypes, people, eventRegistrationGroup, churchGroup:churchGroups.Last(), count:4);
            
            var events = zonalEvents.Concat(churchGroup1Events).Concat(churchGroup2Events).ToList();
            // Generate event sessions for each event
            foreach (var evt in events)
            {
                var sessions = new Faker<EventSession>()
                    .RuleFor(s => s.EventId, evt.Id)
                    .RuleFor(s => s.Name, f => f.Lorem.Word())
                    .RuleFor(s => s.Description, f => f.Lorem.Sentence())
                    .RuleFor(s => s.Location, f => f.Address.FullAddress())
                    .RuleFor(e => e.AttendanceRequired, f => faker.Random.Bool(0.3f))
                    .RuleFor(e => e.Capacity, f => f.Random.Number(50, 200))
                    .RuleFor(e => e.OnlineSupport,
                        f => f.PickRandom(OnlineSupport.NotOnline, OnlineSupport.Both, OnlineSupport.OnlineOnly))
                    .Generate(faker.Random.Number(1, 5));
                
                // Add Session Order and Online Meeting Urls
                int sessionOrder = 1;
                sessions.ForEach(x =>
                {
                    x.SessionOrder = sessionOrder; sessionOrder++;
                    x.OnlineMeetingUrl = x.OnlineSupport == OnlineSupport.NotOnline ? null : faker.Internet.Url();
                });
                
                var orderedSessions = sessions.OrderBy(x => x.SessionOrder).ToList();
                // Add session Schedule Dates, based on the Session Order
                for (int i = 0; i < orderedSessions.Count; i++)
                {
                    var sessionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(i));
                    var sessionTime = new TimeOnly(14,0); //14:00:00
                    orderedSessions[i].Schedule = new()
                    {
                        Name = $"{orderedSessions[i].Name}-EventSession-Schedule",
                        StartDate = sessionDate,
                        EndDate = sessionDate,
                        StartTime = sessionTime,
                        EndTime = sessionTime.Add(new TimeSpan(i+1, 0, 0)), // Add i hours
                        Timezone = "South Africa Standard Time"
                    };
                }

                evt.Sessions = orderedSessions;

                /*evt.Sessions = sessions.OrderBy(x => x.StartDateTime).ToList();

                var startDateTime = evt.Sessions.First().StartDateTime;
                var endDateTime = evt.Sessions.Last().EndDateTime;

                var calendar = InetCalendarHelper.CreateCalendarWithRecurrence(
                    FrequencyType.Daily,
                    startDateTime.Value,
                    durationMinutes: faker.Random.Number(60, 240),
                    endDateTime,
                    timezone: "South Africa Standard Time");*/
            }

            await dbContext.Event.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();
        }
    }

    private static List<Event> GenerateEvents(
        List<EventType> eventTypes, List<Person> people, Group eventRegistrationGroup, ChurchGroupAndChurches churchGroup = null, int count = 10)
    {
        var events = new Faker<Event>()
            .RuleFor(e => e.Name, f => string.Join(" ", f.Lorem.Words(3)))
            .RuleFor(e => e.Description, f => f.Lorem.Sentence())
            .RuleFor(e => e.EventTypeId, f => f.PickRandom(eventTypes).Id)
            .RuleFor(e => e.ChurchGroupId, f => churchGroup?.ChurchesGroupId)
            .RuleFor(e => e.ChurchId, f =>  churchGroup == null ? null : f.PickRandom(churchGroup.ChurchIds))
            .RuleFor(e => e.ContactPersonId, f => f.PickRandom(people).Id)
            .RuleFor(e => e.ContactEmail, f => f.Person.Email)
            .RuleFor(s => s.Location, f => f.Address.FullAddress())
            .RuleFor(e => e.ContactPhone, f => f.Person.Phone)
            .RuleFor(e => e.ApprovalStatus, f => ApprovalStatus.Approved)   
            .RuleFor(e => e.EventRegistrationGroup, f => eventRegistrationGroup)
            .RuleFor(e => e.ApprovalStatus,
                f => f.PickRandom(ApprovalStatus.PendingApproval, ApprovalStatus.Denied, ApprovalStatus.Approved))
            .RuleFor(e => e.Capacity, f => f.Random.Number(50, 500))
            .RuleFor(e => e.PhotoUrl, f => f.Image.PicsumUrl())
            //.RuleFor(e => e.HasChildCare, f => f.Random.Bool())
            //.RuleFor(e => e.MinChildAge, (f, e) => e.HasChildCare ? f.Random.Number(0, 5) : null)
            //.RuleFor(e => e.MaxChildAge, (f, e) => e.HasChildCare ? f.Random.Number((int)e.MinChildAge + 1, 12) : null)
            .Generate(count);
        return events;
    }
}

class ChurchGroupAndChurches
{
    public int ChurchesGroupId { get; set; }
    public IList<int> ChurchIds { get; set; }
}