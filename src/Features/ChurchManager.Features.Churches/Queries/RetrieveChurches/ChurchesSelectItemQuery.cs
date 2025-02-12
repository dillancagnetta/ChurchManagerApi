using AutoMapper;
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
        private readonly IMapper _mapper;

        public AllChurchQueryHandler(
            IGenericDbRepository<Church> dbRepository,
            ITenantCurrentUser currentUser,
            IPermissionService permissions,
            IMapper mapper)
        {
            _dbRepository = dbRepository;
            _currentUser = currentUser;
            _permissions = permissions;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(ChurchesQuery query, CancellationToken ct)
        {
            /*var vm = await _mapper
                .ProjectTo<ChurchViewModel>(_dbRepository.Queryable().OrderBy(x => x.Name))
                .ToListAsync(ct);*/

            var spec = new ChurchesListSpecification(Guid.Parse(_currentUser.Id), _permissions, query.SearchTerm);
            
            var vm = await _dbRepository.ListAsync(spec, ct);
            
            /*var vm = await _dbRepository.Queryable().OrderBy(x => x.Name)
                .MapTo<Church, ChurchViewModel>()
                .ToListAsync(ct);*/

            return new ApiResponse(vm);
        }
    }
}