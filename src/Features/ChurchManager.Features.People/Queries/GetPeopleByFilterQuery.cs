using ChurchManager.Application.Abstractions.Services;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.People.Queries;

public record GetPeopleByFilterQuery : IRequest<ApiResponse>
{
    public IList<int> PersonIds { get; set; } = [];
}

public class GetPeopleByFilterHandler(
    IPersonService service) : IRequestHandler<GetPeopleByFilterQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetPeopleByFilterQuery query, CancellationToken cancellationToken)
    {
        var vm = await service.FilterPeopleAsync(query.PersonIds, cancellationToken);
        
        return new ApiResponse(vm);
    }
}