using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Events;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.People.Queries.FindDuplicates;

public record FindPeopleDuplicatesQuery : IRequest<ApiResponse>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public string PhoneNumber { get; set; }
}

public class PeopleDuplicateHandler : IRequestHandler<FindPeopleDuplicatesQuery, ApiResponse>
{
    private readonly IPersonDbRepository _dbRepository;

    public PeopleDuplicateHandler(IPersonDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task<ApiResponse> Handle(FindPeopleDuplicatesQuery query, CancellationToken ct)
    {
        var searchParams = new PersonMatchQuery(query.FirstName, query.LastName, query.Email, null);
            
        var spec = new FindPeopleSpecification(searchParams);

        var people = await _dbRepository.ListAsync(spec, ct);

        return new ApiResponse(people?.Any() ?? false);
    }
}

/*
 * -----------------------------
 */
public record VerifyPersonExistsQuery : FindPeopleDuplicatesQuery;

public class VerifyPersonExistsHandler(
    IMediator mediator,
    IPersonDbRepository dbRepository,
    IDomainEventPublisher publisher) : IRequestHandler<VerifyPersonExistsQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(VerifyPersonExistsQuery query, CancellationToken ct)
    {
        var _query = new FindPeopleDuplicatesQuery
        {
            Email = query.Email,
            FirstName = query.FirstName,
            LastName = query.LastName
        };
        var response = await mediator.Send(_query, ct);

        if (response.Succeeded && response.Data == true)
        {
            var person = await dbRepository
                .FindPersons(
                    new PersonMatchQuery(query.FirstName, query.LastName, query.Email, null),
                    includes: ["Family"]
                )
                .Select(x => new
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    FamilyCode = x.Family.Code,
                    Email = x.Email
                })
                .FirstOrDefaultAsync(ct);
            
            var templateData = new Dictionary<string, string>
            {
                ["Title"] = person!.FullName.Title,
                ["FirstName"] = person!.FullName.FirstName,
                ["LastName"] = person!.FullName.LastName,
                ["FamilyCode"] = person!.FamilyCode,
                ["CreationDate"] = DateTime.UtcNow.ToShortTimeString()
            };
            
            var recipient = new EmailRecipient
            {
                PersonId = person.Id,
                EmailAddress = person.Email.Address
            };
            
            await publisher.PublishAsync(new SendEmailEvent(
                "Family Code",
                DomainConstants.Communication.Email.Templates.FamilyCodeRequest,
                recipient)
            {
                TemplateData = templateData
            }, ct);
        }
        
        return response;
    }
}
