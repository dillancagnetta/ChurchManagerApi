using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using Convey.CQRS.Queries;

namespace ChurchManager.Application.Abstractions.Services;

public interface IChurchService : ICrudServiceAsync<Church, ChurchViewModel, EditChurchModel>
{
    Task<IReadOnlyList<ChurchViewModel>> ChurchListAsync(
        string searchTerm,
        int? churchGroupId,
        CancellationToken ct = default);
    
    Task<PagedResult<ChurchAttendanceViewModel>> BrowseChurchAttendance(
        QueryParameter query, 
        int[] attendanceTypeIds,
        int churchId,
        int? churchGroupId,
        bool withFeedback,
        DateTime? from, DateTime? to,
        CancellationToken ct = default);
}