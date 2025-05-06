using ChurchManager.Infrastructure.Abstractions;

namespace ChurchManager.Domain.Features.Groups.Events
{
    public record GroupAttendanceReviewedEvent(int AttendanceId, int GroupId) : IDomainEvent
    {
        public string? Feedback { get; set; }
    }
}
