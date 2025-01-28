using ChurchManager.SharedKernel.Wrappers;
using MediatR;

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

public class RegisterFamilyForEventHandler : IRequestHandler<RegisterFamilyForEventCommand, ApiResponse>
{
    public Task<ApiResponse> Handle(RegisterFamilyForEventCommand command, CancellationToken ct)
    {
        return Task.FromResult(new ApiResponse {Succeeded = true, Message = "Registration successful!"});
    }
}