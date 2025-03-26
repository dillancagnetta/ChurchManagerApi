using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Events.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.Extensions;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class EventDbRepository(ChurchManagerDbContext dbContext) : GenericRepositoryBase<Event>(dbContext), IEventDbRepository
{
    public async Task<EventViewModel> EventDetailsAsync(int eventId, CancellationToken ct = default)
    {
        var vm = await Queryable()
            .AsNoTracking()
            .Include(x => x.Church)
            .Include(x => x.ChurchGroup)
            .Include(x => x.EventType)
            .Include(x => x.ContactPerson)
            .Include(x => x.Sessions)
                .ThenInclude(x => x.Schedule)
            .Include(x => x.EventRegistrationGroup)
                .ThenInclude(x => x.GroupType)
            .Include(x => x.ChildCareGroup)
             .ThenInclude(x => x.GroupType)
            .FirstOrDefaultAsync(x => x.Id == eventId, ct)
            .Select(x => new EventViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                PhotoUrl = x.PhotoUrl,
                EventTypeId = x.EventTypeId,
                EventTypeName = x.EventType?.Name,
                ChurchReference = new ChurchReference
                {
                    ChurchId = x.ChurchId,
                    ChurchGroupId = x.ChurchGroupId,
                    ChurchName = x.Church?.Name,
                    ChurchGroupName = x.ChurchGroup?.Name
                },
                Location = x.Location,
                ContactPerson = x.ContactPerson != null ? new PersonViewModelBasic
                {
                    PersonId = x.ContactPerson.Id,
                    Gender = x.ContactPerson.Gender,
                    FirstName = x.ContactPerson.FullName.FirstName,
                    LastName = x.ContactPerson.FullName.LastName,
                    AgeClassification = x.ContactPerson.AgeClassification,
                    Age = x.ContactPerson.BirthDate.Age,
                    PhotoUrl = x.ContactPerson.PhotoUrl
                } : null,
                Configuration = new EventConfigurationViewModel
                {
                    OnlineSupport = x.EventType?.OnlineSupport,
                    RequiresRegistration = x.EventType.RequiresRegistration,
                    AllowFamilyRegistration = x.EventType.AllowFamilyRegistration,
                    AllowNonFamilyRegistration = x.EventType.AllowNonFamilyRegistration,
                    RequiresChildInfo = x.EventType.RequiresChildInfo,
                    TakesAttendance = x.EventType.TakesAttendance,
                    HasChildCare = x.EventType.ChildCare?.HasChildCare,
                    MinChildAge = x.EventType.ChildCare?.MinChildAge,   
                    MaxChildAge = x.EventType.ChildCare?.MaxChildAge,
                },
                EventRegistrationGroup = x.EventRegistrationGroupId.HasValue ? new GroupReference
                {
                    GroupId = x.EventRegistrationGroupId,
                    GroupName = x.EventRegistrationGroup?.Name,
                    GroupTypeId = x.EventRegistrationGroup?.GroupTypeId,
                    GroupTypeName = x.EventRegistrationGroup?.GroupType?.Name
                } : null,
                ChildCareGroup = x.ChildCareGroupId.HasValue ? new GroupReference
                {
                    GroupId = x.ChildCareGroupId,
                    GroupName = x.ChildCareGroup?.Name,
                    GroupTypeId = x.ChildCareGroup?.GroupTypeId,
                    GroupTypeName = x.ChildCareGroup?.GroupType?.Name
                } : null,
                NumberOfSessions = x.Sessions.Count,
                Sessions = x.Sessions?.Select(x => new EventSessionViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    SessionOrder = x.SessionOrder,
                    StartDate = x.SessionStartDateTime().StartDate,
                    StartTime = x.SessionStartDateTime().StartTime?.ToTimeUtcString(),
                    EndDate = x.SessionEndDateTime().EndDate,
                    EndTime = x.SessionEndDateTime().EndTime?.ToTimeUtcString(),
                    Location = x.Location,
                    OnlineSupport = x.OnlineSupport,
                    OnlineMeetingUrl = x.OnlineMeetingUrl,
                    AttendanceRequired = x.AttendanceRequired,
                })
            });
        
        return vm;
    }
} 
