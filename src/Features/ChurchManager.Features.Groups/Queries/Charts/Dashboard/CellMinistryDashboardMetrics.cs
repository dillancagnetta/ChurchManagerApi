using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Groups.Queries.Charts.Dashboard
{
    public class CellMinistryDashboardMetrics : IRequest<ApiResponse>
    {
    }

    public class CellMinistryDashboardMetricsHandler : IRequestHandler<CellMinistryDashboardMetrics, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;
        private readonly IGroupMemberDbRepository _groupMemberDbRepository;
        private readonly IGroupMemberAttendanceDbRepository _groupMemberAttendanceDbRepository;
        private readonly IGroupAttendanceDbRepository _groupAttendanceDb;
        private readonly IGroupTypeDbRepository _groupTypeRepo;

        public CellMinistryDashboardMetricsHandler(
            IGroupDbRepository dbRepository,
            IGroupMemberDbRepository groupMemberDbRepository,
            IGroupMemberAttendanceDbRepository groupMemberAttendanceDbRepository,
            IGroupAttendanceDbRepository groupAttendanceDb,
            IGroupTypeDbRepository groupTypeRepo)
        {
            _dbRepository = dbRepository;
            _groupMemberDbRepository = groupMemberDbRepository;
            _groupMemberAttendanceDbRepository = groupMemberAttendanceDbRepository;
            _groupAttendanceDb = groupAttendanceDb;
            _groupTypeRepo = groupTypeRepo;
        }

        public async Task<ApiResponse> Handle(CellMinistryDashboardMetrics query, CancellationToken ct)
        {
            var cellGroupType = await _groupTypeRepo.GetCellGroupTypeAsync(ct);

            // TODO: fix doing this sql here and in the 'GroupStatisticsAsync' method again
            var cellGroups = await _dbRepository.Queryable()
                .AsNoTracking()
                .Where(x => x.GroupTypeId == cellGroupType.Id)
                .Select(x => new {x.Id})
                .ToListAsync(ct);

            // TODO: create class/structs for these
            var (totalCellsCount, activeCellsCount, inActiveCellsCount, onlineCellsCount, openedCells, closedCells) = await _dbRepository.GroupStatisticsAsync(cellGroupType.Id, DateTime.UtcNow.AddMonths(-6), ct);

            var (peopleCount, leadersCount) = await _groupMemberDbRepository.PeopleAndLeadersInGroupsAsync(cellGroupType.Id, ct);

            var (newConvertsCount, firstTimersCount, holySpiritCount) = await _groupAttendanceDb.PeopleStatisticsAsync(cellGroups.Select(x => x.Id), PeriodType.ThisYear, ct);

            return new ApiResponse(new
            {
                totalCellsCount, activeCellsCount, inActiveCellsCount, onlineCellsCount, peopleCount, leadersCount, openedCells, closedCells,
                newConvertsCount,
                firstTimersCount,
                holySpiritCount
            });
        }
    }
}
