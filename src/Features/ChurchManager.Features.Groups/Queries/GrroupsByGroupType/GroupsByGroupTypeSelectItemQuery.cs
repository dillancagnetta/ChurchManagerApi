using AutoMapper;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Groups.Queries.GrroupsByGroupType
{
    public record GroupsByGroupTypeSelectItemQuery : IRequest<ApiResponse>
    {
        public int GroupTypeId { get; set; }
        public int? ChurchId { get; set; }
        public bool IncludeParentGroups { get; set; } = true;
    }

    public class GroupsByGroupTypeSelectItemHandler : IRequestHandler<GroupsByGroupTypeSelectItemQuery, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public GroupsByGroupTypeSelectItemHandler(IGroupDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(GroupsByGroupTypeSelectItemQuery query, CancellationToken ct)
        {
            var queryable = _dbRepository
                .Queryable()
                .AsNoTracking()
                .Where(x => x.GroupTypeId == query.GroupTypeId)
                .Where(x => query.IncludeParentGroups || x.ParentGroupId != null);

            if (query.ChurchId.HasValue && query.ChurchId.Value > 0)
            {
                queryable = queryable.Where(x => x.ChurchId == query.ChurchId.Value);
            }
            
            var vm = await _mapper
                .ProjectTo<SelectItemViewModel>(
                    queryable.OrderBy(x => x.Name)
                    )
                .ToListAsync(ct);

            return new ApiResponse(vm);
        }
    }
}
