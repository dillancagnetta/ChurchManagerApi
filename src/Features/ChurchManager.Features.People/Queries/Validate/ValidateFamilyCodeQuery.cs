using ChurchManager.Domain.Features.Communication.Events;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.People.Queries.Validate;

public record ValidateFamilyCodeQuery(string FamilyCode) : IRequest<ApiResponse>;

public class ValidateFamilyCodeHandler(IFamilyDbRepository dbRepository) : IRequestHandler<ValidateFamilyCodeQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(ValidateFamilyCodeQuery request, CancellationToken cancellationToken)
    {
        return new ApiResponse(
            await dbRepository.ValidateFamilyCodeAsync(request.FamilyCode.ToUpperInvariant(), cancellationToken)
            );
    }
}


public record RequestFamilyCodeCommand(string EmailAddress) : IRequest<ApiResponse>;

public class RequestFamilyCodeHandler(
    IPersonDbRepository dbRepository,
    IDomainEventPublisher publisher) : IRequestHandler<RequestFamilyCodeCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(RequestFamilyCodeCommand command, CancellationToken cancellationToken)
    {
        var person = dbRepository
            .Queryable()
            .Include(x => x.Family)
            .AsNoTracking()
            .FirstOrDefault(x=> 
                x.Email != null && x.Email.Address != null 
                && x.Email.Address == command.EmailAddress);

        if (person != null)
        {
            var templateData = new Dictionary<string, string>
            {
                ["Title"] = person!.FullName.Title,
                ["FirstName"] = person!.FullName.FirstName,
                ["LastName"] = person!.FullName.LastName,
                ["FamilyCode"] = person!.Family.Code,
                ["CreationDate"] = DateTime.UtcNow.ToShortTimeString()
            };
            
            await publisher.PublishAsync(new SendEmailEvent(
                "Family Code Request",
                DomainConstants.Communication.Email.Templates.FamilyCodeRequest,
                command.EmailAddress)
            {
                TemplateData = templateData
            }, cancellationToken);
        }
        
        return new ApiResponse();
    }
}