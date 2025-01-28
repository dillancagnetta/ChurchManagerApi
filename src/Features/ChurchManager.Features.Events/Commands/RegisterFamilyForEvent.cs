using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using Codeboss.Types;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Events.Commands;

public record RegisterFamilyForEventCommand(int EventId, IEnumerable<SelectedMember> SelectedMembers) : IRequest<ApiResponse>;

public record SelectedMember
{
    public int PersonId { get; set; }
    public bool IsSelected { get; set; }
    public ChildInfo ChildInfo { get; set; }
    public Dictionary<int, SessionPreference> SessionPreferences { get; set; }
}

public record ChildInfo
{
    // Add properties as needed for child information
    // For example:
    // public string EmergencyContact { get; set; }
    // public string AllergiesInfo { get; set; }
}

public record SessionPreference
{
    public bool? AttendingOnline { get; set; }
    public bool? AttendingInPerson { get; set; }
}

public class RegisterFamilyForEventHandler(
    IGenericDbRepository<Event> eventDb,
    IGroupMemberDbRepository groupMemberDb,
    IDateTimeProvider dateTime) : IRequestHandler<RegisterFamilyForEventCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(RegisterFamilyForEventCommand command, CancellationToken ct)
    {
        // Add to Event EventRegistrations
        // Add to Group (if not null) as Group Members
        // Find the EventSession and add to EventSessionRegistration for those in the session

        try
        {
            var @event = await eventDb
                .Queryable()
                .Include(x => x.Sessions)
                .Include(x => x.Registrations)
                .ThenInclude(x => x.SessionRegistrations)
                .FirstOrDefaultAsync(x => x.Id == command.EventId, ct);

            if (@event is not null)
            {
                var registrants = command.SelectedMembers.Where(x => x.IsSelected).ToList();

                foreach (var registree in registrants)
                {
                    var registration = new EventRegistration
                    {
                        PersonId = registree.PersonId,
                        RegistrationDate = dateTime.Now,
                        RegisteredForAllSessions =
                            registrants.First(x => x.PersonId == registree.PersonId).SessionPreferences.Count ==
                            @event.Sessions.Count,
                        Status = RegistrationStatus.Confirmed.ToString(),
                        GroupId = @event.EventRegistrationGroupId
                    };
                
                    @event.Registrations.Add(registration);
                
                    // Add SessionRegistrations for each session
                    foreach (var session in @event.Sessions)
                    {
                        var sessionsSelected = registree.SessionPreferences.TryGetValue(session.Id, out SessionPreference? sessionPreference);
                        if (sessionsSelected && sessionPreference != null)
                        {
                            bool? attendingOnline = sessionPreference.AttendingOnline;
                            bool? attendingInPerson = sessionPreference.AttendingInPerson;
                            
                            var sessionRegistration = new EventSessionRegistration
                            {
                                RegisteredDate = dateTime.Now,
                                EventSessionId = session.Id,
                                PersonId = registration.PersonId,
                                AttendingOnline = attendingOnline,
                                AttendingInPerson = attendingInPerson
                                //AttendingInPerson = prefer != null && prefer.AttendingInPerson.HasValue && prefer.AttendingInPerson.Value,
                            };
                            registration.SessionRegistrations.Add(sessionRegistration);
                        }
                        
                       
                    }

                    // Register as group members
                    if (@event.EventRegistrationGroupId.HasValue)
                    {
                        await groupMemberDb.AddGroupMember(
                            @event.EventRegistrationGroupId.Value, registree.PersonId,
                            4, ct: ct);
                    }
                }
            
                await eventDb.SaveChangesAsync(ct);
                return new ApiResponse { Succeeded = true, Message = "Registration successful!" };
            }

            return new ApiResponse("Event not found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}