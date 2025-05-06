using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.ViewModels
{
    public record GroupAttendanceDetailViewModel : GroupAttendanceViewModel
    {
        // Helpers
        public bool AttendanceReviewed { get; set; }
        public bool AttendanceEntered { get; set; }
        public int DidAttendCount { get; set; }
        public AttendanceReview AttendanceReview { get; set; }
    }
}