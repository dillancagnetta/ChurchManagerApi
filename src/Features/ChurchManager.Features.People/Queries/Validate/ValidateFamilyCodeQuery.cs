using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

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