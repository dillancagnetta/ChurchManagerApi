using AutoFixture;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.People;

namespace ChurchManager.Domain.Tests;

public class Event_DomainTest
{
    private readonly IFixture _fixture;

    public Event_DomainTest()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Test_event_setup()
    { 
        var church = _fixture.Create<Church>();
        var churchGroup = _fixture.Create<ChurchGroup>();
        var personRegistering1 = _fixture.Create<Person>();
        var personRegistering2 = _fixture.Create<Person>();

        // Create EventType using property initializer
        var eventType = new EventType
        {
            Name = "Annual Prayer Conference",
            Description = "A special event for prayer.",
            RequiresRegistration = true,
            AllowFamilyRegistration = true,
            TakesAttendance = true,
            RequiresChildInfo = false,
            OnlineSupport = OnlineSupport.Both,
        };

        // Create Event using property initializer
        var eventSessionRegistrations = new List<EventSessionRegistration>()
        {
            new ()
            {
                RegisteredDate = DateTime.Now,
                EventRegistration = new EventRegistration()
                {
                    Id = 1,
                    Person = personRegistering1,
                    GroupId = 1,
                    EventId = 1,
                    Status = RegistrationStatus.Confirmed.ToString(),
                }
            },
            new ()
            {
                RegisteredDate = DateTime.Now,
                EventRegistration = new EventRegistration()
                {
                    Id = 2,
                    Person = personRegistering2,
                    GroupId = 1,
                    EventId = 1,
                    Status = RegistrationStatus.Confirmed.ToString(),
                }
            }
        };
        var event1 = new Event
        {
            Id = 1,
            Name = "2025 Annual Prayer Conference",
            Description = "A special event for prayer.",
            Church = church,
            ChurchGroup = churchGroup,
            ScheduleId = 1,
            ChildCareGroupId = 4,
            EventType = eventType,
            Capacity = 500,
            /*Sessions = new List<EventSession> {
                new ()
                {
                    Id = 1, Name = "Session 1",
                    SessionSchedules = new List<EventSessionSchedule>( )
                    {
                        new ()
                        {
                            StartDateTime = DateTime.Now.AddDays(1),
                            EndDateTime = DateTime.Now.AddDays(1).AddHours(3),
                            Location = "Main Auditorium",
                            SessionRegistrations = eventSessionRegistrations
                        }
                    }
                } 
            }*/
        };

        // Assertions
        Assert.NotNull(event1);
        Assert.Equal("2025 Annual Prayer Conference", event1.Name);
        Assert.Equal(church.Id, event1.Church.Id);
        Assert.Equal(church.Name, event1.Church.Name);
        Assert.Equal(churchGroup.Id, event1.ChurchGroup.Id);
        Assert.Equal(churchGroup.Name, event1.ChurchGroup.Name);
        Assert.Equal("Annual Prayer Conference", event1.EventType.Name);
        Assert.True(event1.EventType.RequiresRegistration);
        Assert.Equal(500, event1.Capacity);
    }
}