using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Churches.Specifications;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.MultiTenant;
using MediatR;

namespace ChurchManager.Features.Churches.Queries.RetrieveChurches;

public class ChurchesQuery : IRequest<ApiResponse>
{
    public int? ChurchGroupId { get; set; }
    public string? SearchTerm { get; set; }
}

public class AllChurchQueryHandler : IRequestHandler<ChurchesQuery, ApiResponse>
{
    private readonly IGenericDbRepository<Church> _dbRepository;
    private readonly ITenantCurrentUser _currentUser;
    private readonly IPermissionService _permissions;
    private readonly IChurchService _service;
    private readonly IMapper _mapper;

    public AllChurchQueryHandler(
        IGenericDbRepository<Church> dbRepository,
        ITenantCurrentUser currentUser,
        IPermissionService permissions,
        IChurchService service,
        IMapper mapper)
    {
        _dbRepository = dbRepository;
        _currentUser = currentUser;
        _permissions = permissions;
        _service = service;
        _mapper = mapper;
    }

    public async Task<ApiResponse> Handle(ChurchesQuery query, CancellationToken ct)
    {
        /*var vm = await _mapper
            .ProjectTo<ChurchViewModel>(_dbRepository.Queryable().OrderBy(x => x.Name))
            .ToListAsync(ct);*/
            
        // Get all allowed entity IDs
        /*var allowedIds = await _permissions.GetAllowedEntityIdsAsync<Church>(Guid.Parse(_currentUser.Id), "View", ct);
        var spec = new ChurchesListSpecification(allowedIds,  query.SearchTerm);

        var vm = await _dbRepository.ListAsync(spec, ct);*/
            
        /*var vm = await _dbRepository.Queryable().OrderBy(x => x.Name)
            .MapTo<Church, ChurchViewModel>()
            .ToListAsync(ct);*/
            
        var vm = await _service.ChurchListAsync(query.SearchTerm, query.ChurchGroupId, ct);
        return new ApiResponse(vm);
    }
}

/*
 * -----------------------------------------------------------------------------------
 */
 
public record AddChurchCommand(string Name, string Description, int ChurchGroupId, int? LeaderPersonId) : IRequest<ApiResponse>;
public class AddChurchCommandHandler(
    IChurchService service,
    IPersonDbRepository personDb) : IRequestHandler<AddChurchCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(AddChurchCommand command, CancellationToken ct)
    {
        /*var dto = new ChurchViewModel
        {
            Name = command.Name,
            Description = command.Description,
            ChurchGroupId = command.ChurchGroupId,
            LeaderPerson = command.LeaderPersonId.HasValue
                // Needed because we rerender the list - so we need this data
                ? await personDb.BasicPersonViewModelAsync(command.LeaderPersonId.Value, ct)
                : null
        };
        
        var entity = await service.AddAsync(dto, ct);
        dto.Id = entity.Id;*/
        
        var dto = new EditChurchModel
        {
            Name = command.Name,
            Description = command.Description,
            ChurchGroupId = command.ChurchGroupId,
            LeaderPersonId = command.LeaderPersonId
        };

        var vm = await service.AddAsync(dto, ct);
        
        //var entity = await service.AddAsync(dto, ct);
        // Needed because we rerender the list - so we need this data
        vm.LeaderPerson = command.LeaderPersonId.HasValue
            ? await personDb.BasicPersonViewModelAsync(command.LeaderPersonId.Value, ct)
            : null;

        return new ApiResponse(vm);
    }
}

/*
 * ------------------------------------------------
 */

public record DeleteChurchCommand(int ChurchGroupId) : IRequest<ApiResponse>;
public class DeleteChurchCommandHandler(
    IChurchService service,
    IPersonDbRepository personDb) : IRequestHandler<DeleteChurchCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(DeleteChurchCommand command, CancellationToken ct)
    {
        await service.DeleteAsync(command.ChurchGroupId, ct);

        return new ApiResponse();
    }
}

/*
 * ------------------------------------------------
 */

public record EditChurchCommand(int Id, string Name, string Description, int? LeaderPersonId, int ChurchGroupId) : IRequest<ApiResponse>;
public class EditChurchCommandHandler(
    IChurchService service,
    IPersonDbRepository personDb) : IRequestHandler<EditChurchCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(EditChurchCommand command, CancellationToken ct)
    {
        var dto = new EditChurchModel
        {
            Id = command.Id,
            Name = command.Name,
            Description = command.Description,
            LeaderPersonId = command.LeaderPersonId,
            ChurchGroupId = command.ChurchGroupId
        };
        
        await service.UpdateAsync(dto, ct);

        return new ApiResponse();
    }
}