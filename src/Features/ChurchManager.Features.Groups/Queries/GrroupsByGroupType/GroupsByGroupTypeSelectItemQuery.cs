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
            var vm = await _mapper
                .ProjectTo<SelectItemViewModel>(
                    _dbRepository
                        .Queryable()
                        .AsNoTracking()
                        .Where(x => x.GroupTypeId == query.GroupTypeId)
                        .Where(x => query.IncludeParentGroups || x.ParentGroupId != null)
                        .OrderBy(x => x.Name)
                    )
                .ToListAsync(ct);

            return new ApiResponse(vm);
        }
    }
}
