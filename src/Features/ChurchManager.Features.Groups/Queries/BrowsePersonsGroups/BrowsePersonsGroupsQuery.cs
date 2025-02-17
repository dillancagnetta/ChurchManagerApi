﻿using AutoMapper;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Groups.Queries.BrowsePersonsGroups
{
    public record BrowsePersonsGroupsQuery
        : SearchTermQueryParameter, IRequest<PagedResponse<PersonGroupsSummaryViewModel>>
    {
        public int PersonId { get; set; }
    }

    public class
        BrowsePersonsGroupsHandler : IRequestHandler<BrowsePersonsGroupsQuery, PagedResponse<PersonGroupsSummaryViewModel>>
    {
        private readonly IGroupDbRepository _groupDbRepository;
        private readonly IMapper _mapper;

        public BrowsePersonsGroupsHandler(
            IGroupDbRepository groupDbRepository,
            IMapper mapper)
        {
            _groupDbRepository = groupDbRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PersonGroupsSummaryViewModel>> Handle(BrowsePersonsGroupsQuery query,
            CancellationToken ct)
        {
            var spec = new BrowsePersonsGroupsSpecification(query.PersonId, query);

            var pagedResult = await _groupDbRepository.BrowseAsync(query, spec, ct);

            //var viewModels = _mapper.Map<PagedResult<GroupSummaryViewModel>>(pagedResult);

            return new PagedResponse<PersonGroupsSummaryViewModel>(pagedResult);
        }
    }
}