using ChurchManager.Domain.Common;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Churches.Repositories
{
    public interface IChurchAttendanceDbRepository : IGenericDbRepository<ChurchAttendance>
    {
        Task<IEnumerable<ChurchAttendanceAnnualBreakdownVm>> DashboardChurchAttendanceAsync(
            DateTime from, DateTime to, int? churchId = null);
        Task<dynamic> DashboardChurchAttendanceBreakdownAsync(DateTime from, DateTime to);
        Task<AttendanceMetricsComparisonViewModel> AttendanceMetricsComparisonAsync(int? queryChurchId, ReportPeriodType queryPeriodType, CancellationToken ct);
    }
}
