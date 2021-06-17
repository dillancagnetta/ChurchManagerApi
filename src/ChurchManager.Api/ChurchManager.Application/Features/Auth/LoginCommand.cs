﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.ViewModels;
using ChurchManager.Infrastructure.Abstractions.Security;
using ChurchManager.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace ChurchManager.Application.Features.Auth
{
    public record LoginCommand([Required] string Username, [Required] string Password) : IRequest<TokenViewModel>
    {
    }

    public class LoginHandler : IRequestHandler<LoginCommand, TokenViewModel>
    {
        private readonly ChurchManagerDbContext _dbContext;
        private readonly ITokenService _tokens;

        public LoginHandler(ChurchManagerDbContext dbContext, ITokenService tokens)
        {
            _dbContext = dbContext;
            _tokens = tokens;
        }

        public async Task<TokenViewModel> Handle(LoginCommand request, CancellationToken ct)
        {
            // get account from database
            var user = await _dbContext.UserLogin
                .FirstOrDefaultAsync(u => u.Username == request.Username, ct);

            // check account found and verify password
            if (user is not null && BC.Verify(request.Password, user.Password))
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, request.Username),
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new("PersonId", user.PersonId.ToString())
                };
                // Roles
                foreach (var role in user.Roles)
                {
                    claims.Add(new(ClaimTypes.Role, role));
                }

                var accessToken = _tokens.GenerateAccessToken(claims);
                var refreshToken = _tokens.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                await _dbContext.SaveChangesAsync(ct);

                return new TokenViewModel(true, accessToken, refreshToken);
            }

            // authentication failed
            return new TokenViewModel(false);
        }
    }
}