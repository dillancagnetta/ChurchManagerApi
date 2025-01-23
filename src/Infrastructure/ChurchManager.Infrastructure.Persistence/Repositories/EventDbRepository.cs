using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Events.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
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
            .Include(x => x.Schedule)
            .Include(x => x.EventType)
            .Include(x => x.Sessions)
            .FirstOrDefaultAsync(x => x.Id == eventId, ct)
            .Select(x => new EventViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                PhotoUrl = x.PhotoUrl,
                EventTypeId = x.EventTypeId,
                EventTypeName = x.EventType?.Name,
                ChurchId = x.ChurchId,
                ChurchGroupId = x.ChurchGroupId,
                ChurchName = x.Church?.Name,
                ChurchGroupName = x.ChurchGroup?.Name,
                Location = x.Location,
                ScheduleFriendlyText = x.Schedule?.ToFriendlyScheduleText(false),
                StartDate = x.Schedule.StartDate.GetValueOrDefault(),
                EndDate =  x.Schedule.StartDate.GetValueOrDefault(),
                Configuration = new EventConfiguration
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

                NumberOfSessions = x.Sessions.Count,
                Sessions = x.Sessions?.Select(x => new EventSessionViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    SessionOrder = x.SessionOrder,
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime,
                    Location = x.Location,
                    OnlineSupport = x.OnlineSupport,
                    OnlineMeetingUrl = x.OnlineMeetingUrl,
                    AttendanceRequired = x.AttendanceRequired,
                })
            });
        
        return vm;
    }
} 
