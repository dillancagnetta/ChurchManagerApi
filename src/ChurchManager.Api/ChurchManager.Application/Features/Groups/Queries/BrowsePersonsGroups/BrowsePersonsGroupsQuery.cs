﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Common;
using ChurchManager.Application.ViewModels;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Parameters;
using Convey.CQRS.Queries;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Queries.BrowsePersonsGroups
{
    public record BrowsePersonsGroupsQuery
        : SearchTermQueryParameter, IRequest<PagedResponse<GroupSummaryViewModel>>
    {
        public int PersonId { get; set; }
    }

    public class BrowsePersonsGroupsHandler : IRequestHandler<BrowsePersonsGroupsQuery, PagedResponse<GroupSummaryViewModel>>
    {
        private readonly ICognitoCurrentUser _currentUser;
        private readonly IGroupDbRepository2 _groupDbRepository;
        private readonly IMapper _mapper;

        public BrowsePersonsGroupsHandler(
            ICognitoCurrentUser currentUser,
            IGroupDbRepository2 groupDbRepository,
            IMapper mapper)
        {
            _currentUser = currentUser;
            _groupDbRepository = groupDbRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<GroupSummaryViewModel>> Handle(BrowsePersonsGroupsQuery query, CancellationToken ct)
        {
            var spec = new BrowsePersonsGroupsSpecification(query.PersonId, query);

            var pagedResult = await _groupDbRepository.BrowseAsync(query, spec, ct);

            var viewModels = _mapper.Map<PagedResult<GroupSummaryViewModel>>(pagedResult);

            return new PagedResponse<GroupSummaryViewModel>(viewModels);
        }
    }
}
