using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Events.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;
using ChurchManager.Domain.Shared;

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
            .Where(x => x.Id == eventId)
            .Select(x => new EventViewModel
            {
               Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                PhotoUrl = x.PhotoUrl,
                EventTypeId = x.EventTypeId,
                EventTypeName = x.EventType != null ? x.EventType.Name : null,
                ChurchReference = new ChurchReference
                {
                    ChurchId = x.ChurchId,
                    ChurchGroupId = x.ChurchGroupId,
                    ChurchName =  x.Church != null ? x.Church.Name : null,
                    ChurchGroupName =  x.ChurchGroup != null ? x.ChurchGroup.Name : null
                },
                Location = x.Location,
                ContactPerson = x.ContactPerson != null ? new PersonViewModelBasic
                {
                    PersonId = x.ContactPerson.Id,
                    Gender = x.ContactPerson.Gender,
                    FirstName = x.ContactPerson.FullName.FirstName,
                    LastName = x.ContactPerson.FullName.LastName,
                    AgeClassification = x.ContactPerson.AgeClassification.Value,
                    Age = x.ContactPerson.BirthDate.Age,
                    PhotoUrl = x.ContactPerson.PhotoUrl
                } : null,
                Configuration = new EventConfigurationViewModel
                {
                    OnlineSupport = x.EventType.OnlineSupport,
                    RequiresRegistration = x.EventType.RequiresRegistration,
                    AllowFamilyRegistration = x.EventType.AllowFamilyRegistration,
                    AllowNonFamilyRegistration = x.EventType.AllowNonFamilyRegistration,
                    RequiresChildInfo = x.EventType.RequiresChildInfo,
                    TakesAttendance = x.EventType.TakesAttendance,
                    HasChildCare = x.EventType.ChildCare != null ? x.EventType.ChildCare.HasChildCare : null,
                    MinChildAge = x.EventType.ChildCare != null ? x.EventType.ChildCare.MinChildAge : null,   
                    MaxChildAge = x.EventType.ChildCare != null ? x.EventType.ChildCare.MaxChildAge : null,
                },
                EventRegistrationGroup = x.EventRegistrationGroupId.HasValue ? new GroupReference
                {
                    GroupId = x.EventRegistrationGroupId,
                    GroupName = x.EventRegistrationGroup != null ? x.EventRegistrationGroup.Name : null,
                    GroupTypeId = x.EventRegistrationGroup != null ? x.EventRegistrationGroup.GroupTypeId : null,
                    GroupTypeName = x.EventRegistrationGroup != null && x.EventRegistrationGroup.GroupType != null
                        ? x.EventRegistrationGroup.GroupType.Name : null
                } : null,
                ChildCareGroup = x.ChildCareGroupId.HasValue ? new GroupReference
                {
                    GroupId = x.ChildCareGroupId,
                    GroupName = x.ChildCareGroup != null ? x.ChildCareGroup.Name : null,
                    GroupTypeId = x.ChildCareGroup != null ? x.ChildCareGroup.GroupTypeId : null,
                    GroupTypeName = x.ChildCareGroup != null && x.ChildCareGroup.GroupType != null
                        ? x.ChildCareGroup.GroupType.Name : null
                } : null,
                NumberOfSessions = x.Sessions.Count,
                Sessions = x.Sessions != null && x.Sessions.Any() ? x.Sessions.Select(x => new EventSessionViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    SessionOrder = x.SessionOrder,
                    StartDate = x.SessionStartDateTime().StartDate,
                    StartTime = x.SessionStartDateTime().StartTime.HasValue ? x.SessionStartDateTime().StartTime.Value.ToTimeUtcString() : null,
                    EndDate = x.SessionEndDateTime().EndDate,
                    EndTime = x.SessionEndDateTime().EndTime.HasValue ? x.SessionEndDateTime().EndTime.Value.ToTimeUtcString() : null,
                    Location = x.Location,
                    OnlineSupport = x.OnlineSupport,
                    OnlineMeetingUrl = x.OnlineMeetingUrl,
                    AttendanceRequired = x.AttendanceRequired,
                }) : Array.Empty<EventSessionViewModel>()
            })
            .FirstOrDefaultAsync(ct);
        
        
        
        return vm;
    }
} 
