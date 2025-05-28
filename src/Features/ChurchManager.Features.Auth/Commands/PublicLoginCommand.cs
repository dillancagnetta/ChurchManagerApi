using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Security;
using MediatR;

namespace ChurchManager.Features.Auth.Commands;

public record PublicLoginCommand([Required] string AccessCode) : IRequest<TokenViewModel>;

public class PublicLoginHandler(
    IFamilyDbRepository familyDb,
    ITokenService tokens) : IRequestHandler<PublicLoginCommand, TokenViewModel>
{
    public async Task<TokenViewModel> Handle(PublicLoginCommand command, CancellationToken ct)
    {
        // get account from database
        var operationResult = await familyDb.FamilyByCodeAsync(command.AccessCode, ct);
        var family = operationResult.Result;

        // check account found and verify password
        if (operationResult.IsSuccess && family is not null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, family.Name),
                new("FamilyId", family.Id.ToString()),
                new(ClaimTypes.Role, "Public Access"),
            };

            var accessToken = tokens.GenerateAccessToken(claims);
            
            return new TokenViewModel(true, accessToken);
        }

        // authentication failed
        return new TokenViewModel(false);
    }
}
