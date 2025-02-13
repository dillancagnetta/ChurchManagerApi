using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Churches.Specifications;
using ChurchManager.Domain.Features.Permissions.Services;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.MultiTenant;
using MediatR;

namespace ChurchManager.Features.Churches.Queries.RetrieveChurches
{
    public class ChurchesQuery : IRequest<ApiResponse>
    {
        public string SearchTerm { get; set; }
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
            
            var vm = await _service.ChurchListAsync(query.SearchTerm, ct);
            return new ApiResponse(vm);

            return new ApiResponse(vm);
        }
    }
}