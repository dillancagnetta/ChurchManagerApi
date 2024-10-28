using ChurchManager.Domain.Features.People.Repositories;
using MediatR;

namespace ChurchManager.Features.Profile.Commands.IncrementProfileViewed;

public record IncrementProfileViewedCommand(int PersonId): IRequest;

public class IncrementProfileViewedHandler : IRequestHandler<IncrementProfileViewedCommand>
{
    private readonly IPersonDbRepository _dbRepository;

    public IncrementProfileViewedHandler(IPersonDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }
    public async Task<Unit> Handle(IncrementProfileViewedCommand request, CancellationToken cancellationToken)
    {
        var person = _dbRepository.Queryable(includeDeceased:false).First(x => x.Id == request.PersonId);
        person.ViewedCount = !person.ViewedCount.HasValue ? 1 : person.ViewedCount.Value + 1;
        await _dbRepository.SaveChangesAsync(cancellationToken);
        
        return new Unit();
    }
}