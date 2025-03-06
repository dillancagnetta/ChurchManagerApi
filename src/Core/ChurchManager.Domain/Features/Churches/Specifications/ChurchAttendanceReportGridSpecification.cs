using Ardalis.Specification;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Specifications;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Churches.Specifications
{
    public class ChurchAttendanceReportGridSpecification : PermissionSpecification<ChurchAttendance, ChurchAttendanceViewModel>
    { 
        // When the ChurchesId is 0 - it means we want to include all churches
        private const int AllChurchesId = 0;

        public ChurchAttendanceReportGridSpecification( 
            int[] attendanceTypeIds,
            int? churchGroupId,
            int[] churchIds,
            DateTime? from, DateTime? to)
        {
            Query.AsNoTracking();
            Query.Include("Church.ChurchGroup");
            Query.Include("ChurchAttendanceType");

            // Type Filter
            if (!attendanceTypeIds.IsNullOrEmpty())
            {
                Query.Where(g => attendanceTypeIds.Contains(g.ChurchAttendanceTypeId));
            }
            
            // Group Filter
            if (churchGroupId.HasValue)
            {
                Query.Where(g => g.Church.ChurchGroupId == churchGroupId);
            }
            
            // Churches Filter
            if (!churchIds.IsNullOrEmpty() && !churchIds.Contains(AllChurchesId))
            {
                Query.Where(g => churchIds.Contains(g.ChurchId));
            }


            // Date Filters
            Query.Where(g => g.AttendanceDate >= from);
            Query.Where(g => g.AttendanceDate <= to);

            // Only show meetings that occurred
            Query.Where(g => g.DidNotOccur == null || g.DidNotOccur.Value == false);

            Query.Select(ExpressionExtensions.SelectChurchAttendance);
        }
    }
}
