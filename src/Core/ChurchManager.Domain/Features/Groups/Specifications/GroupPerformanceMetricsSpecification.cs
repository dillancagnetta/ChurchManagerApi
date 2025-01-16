using Ardalis.Specification;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class GroupPerformanceMetricsSpecification : Specification<GroupMemberAttendance, GroupMemberAttendanceTrackViewModel>
    {
        public GroupPerformanceMetricsSpecification(int groupId, PeriodType period)
        {
            Query.AsNoTracking();

            // Group Filter
            Query.Where(g => g.GroupId == groupId);

            // Date Filters
            var (from, to) = period.ToDateRange();
            Query.Where(g => g.AttendanceDate >= from);
            Query.Where(g => g.AttendanceDate <= to);

            Query.Select(x => new GroupMemberAttendanceTrackViewModel
            {
                GroupId = x.GroupId,
                GroupMemberId = x.GroupMemberId,
                AttendanceDate = x.AttendanceDate,
                DidAttend = x.DidAttend.GetValueOrDefault(),
                IsFirstTime = x.IsFirstTime.GetValueOrDefault(),
                IsNewConvert = x.IsNewConvert.GetValueOrDefault(),
                ReceivedHolySpirit = x.ReceivedHolySpirit.GetValueOrDefault(),
            });
        }
    }
}
