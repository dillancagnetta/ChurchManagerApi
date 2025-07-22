using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Security.Services;
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

    public AllChurchQueryHandler(
        IGenericDbRepository<Church> dbRepository,
        ITenantCurrentUser currentUser,
        IPermissionService permissions,
        IChurchService service)
    {
        _dbRepository = dbRepository;
        _currentUser = currentUser;
        _permissions = permissions;
        _service = service;
    }

    public async Task<ApiResponse> Handle(ChurchesQuery query, CancellationToken ct)
    {
        var vm = await _service.ChurchListAsync(query.SearchTerm, query.ChurchGroupId, ct);
        return new ApiResponse(vm);
    }
}
