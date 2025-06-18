using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.Features;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Churches.Extensions;
using ChurchManager.Domain.Features.Churches.Specifications;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Parameters;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.MultiTenant;
using Convey.CQRS.Queries;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using Codeboss.Results;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Churches.Services;

public class ChurchService(
    IPermissionContext permissions,
    ITenantCurrentUser currentUser,
    IGenericDbRepository<Church> dbRepository,
    IGenericDbRepository<ChurchAttendance> churchAttendanceDb,
    IReadDbRepository<ChurchAttendanceType> attendanceTypeDb,
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

    public async Task<OperationResult<ChurchViewModel>> UpdateChurchAsync(EditChurchModel command, CancellationToken ct = default)
    {
        var attendanceMap = attendanceTypeDb.Queryable().AsNoTracking()
            .Where(x => command.ServiceTimes.Select(x => x.ChurchAttendanceType!).Contains(x.Name))
            .Select(x => new { x.Name , x.Id })
            .ToDictionary(x => x.Name, x => x.Id);

        var church = await dbRepository.Queryable().Include(x => x.ServiceTimes)
            .FirstOrDefaultAsync(x => x.Id == command.Id, ct);;

        if (church == null) return OperationResult<ChurchViewModel>.Fail("Church not found");
        
        church.Name = command.Name; 
        church.Description = command.Description;
        church.LeaderPersonId = command.LeaderPersonId;
        if (command.ChurchGroupId.HasValue) church.ChurchGroupId = command.ChurchGroupId!.Value;
        
        // Services
        var existing = church.ServiceTimes;
        var incomingIds = command.ServiceTimes.Select(x => x.Id).ToHashSet();;
            
        // Step 1: Remove Services that are missing in the DTO
        var toRemove = existing.Where(s => !incomingIds.Contains(s.Id)).ToList();
        foreach (var session in toRemove)
        {
            church.ServiceTimes.Remove(session);
        }
        
        // Step 2: Update existing services without replacing the collection
        foreach (var serviceDto in command.ServiceTimes.Where(s => s.Id is not (null or 0)))
        {
            var existingService = church.ServiceTimes.FirstOrDefault(s => s.Id == serviceDto.Id);
            if (existingService != null)
            {
                existingService.ChurchAttendanceTypeId = attendanceMap[serviceDto.ChurchAttendanceType!];
                existingService.Time = serviceDto.Time;
                existingService.DayOfWeek = serviceDto.DayOfWeek;
            }
        }
        
        // Step 3: Add new Services (Id == 0 or null)
        var newSessions = command.ServiceTimes
            .Where(s => s.Id is null or 0)
            .Select(serviceDto =>
            {
                var newSession = new ChurchServiceTime
                {
                    ChurchAttendanceTypeId = attendanceMap[serviceDto.ChurchAttendanceType!],
                    Time = serviceDto.Time,
                    DayOfWeek = serviceDto.DayOfWeek,
                    Church = church
                };
                return newSession;
            }).ToList();
        
        foreach (var newSession in newSessions)
        {
            church.ServiceTimes.Add(newSession);
        }
        
        await dbRepository.UpdateAsync(church, ct);
        
        // Reload
        // Needed because we rerender the list - so we need this data
        church = await dbRepository.Queryable()
            .AsNoTracking()
            .Include(x => x.LeaderPerson)
            .Include(x => x.ServiceTimes)
                .ThenInclude(s => s.ChurchAttendanceType)
            .FirstAsync(x => x.Id == command.Id, ct);
        
        return new OperationResult<ChurchViewModel>(church.ToViewModel(includeDetails: true));
    }
}