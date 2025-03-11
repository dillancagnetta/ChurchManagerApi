using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.Features;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Churches.Specifications;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.MultiTenant;
using Convey.CQRS.Queries;

namespace ChurchManager.Features.Churches.Services;

public class ChurchService(
    IPermissionContext permissions,
    ITenantCurrentUser currentUser,
    IGenericDbRepository<Church> dbRepository,
    IGenericDbRepository<ChurchAttendance> churchAttendanceDb,
    IMapper mapper) : CrudServiceAsync<Church, ChurchViewModel, EditChurchModel>(dbRepository, mapper), IChurchService
{
    public async Task<IReadOnlyList<ChurchViewModel>> ChurchListAsync(string searchTerm, int? churchGroupId, CancellationToken ct = default)
    {
        var allowedIds = await permissions.GetAllowedIdsAsync<Church>(Guid.Parse(currentUser.Id), PermissionAction.View.Value, ct);
        
        var spec = new ChurchesListSpecification(allowedIds, searchTerm, churchGroupId);
        
        var vm = await Repository.ListAsync(spec, ct);
    
        return vm;
    }

    public async Task<PagedResult<ChurchAttendanceViewModel>> BrowseChurchAttendance(QueryParameter query, 
        int[] attendanceTypeIds,
        int churchId, int? churchGroupId, bool withFeedback, DateTime? from,
        DateTime? to, CancellationToken ct = default)
    {
        var spec = new BrowseChurchAttendanceSpecification(query, attendanceTypeIds, churchId, churchGroupId, withFeedback, from, to);
        
        var pagedResult = await churchAttendanceDb.BrowseAsync(query, spec, ct);
        
        return pagedResult;
    }
}