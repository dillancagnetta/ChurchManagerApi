using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using DotLiquid.Util;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.UserLogins.Commands.AddUserLogin;

public record AddOrUpdateUserLoginCommand : IRequest<ApiResponse>
{
    public int PersonId { get; set; }
    public string Tenant { get; set; }
    public List<string> Roles { get; set; } = new(0);
}

public class AddUserLoginHandler : IRequestHandler<AddOrUpdateUserLoginCommand, ApiResponse>
{
    private readonly IGenericDbRepository<UserLogin> _dbRepository;
    private readonly IGenericDbRepository<UserLoginRole> _roleRepository;
    private readonly IGenericDbRepository<UserRoleAssignment> _roleAssignmentRepository;
    private readonly IPersonDbRepository _personDbRepository;

    public AddUserLoginHandler(
        IGenericDbRepository<UserLogin> dbRepository,
        IGenericDbRepository<UserLoginRole> roleRepository,
        IGenericDbRepository<UserRoleAssignment> roleAssignmentRepository,
        IPersonDbRepository personDbRepository)
    {
        _dbRepository = dbRepository;
        _roleRepository = roleRepository;
        _roleAssignmentRepository = roleAssignmentRepository;
        _personDbRepository = personDbRepository;
    }

    public async Task<ApiResponse> Handle(AddOrUpdateUserLoginCommand command, CancellationToken ct)
    {
        var userLogin = await _dbRepository
            .Queryable()
            .Include(x => x.UserRoles)
                .ThenInclude(ur => ur.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PersonId == command.PersonId, ct);

        // Get or create roles
        var roles = await GetOrCreateRoles(command.Roles, ct);

        if (userLogin is not null)
        {
            userLogin.Tenant = command.Tenant;
                
            // Get existing role assignments to delete
            var existingAssignments = await _roleAssignmentRepository
                .Queryable()
                .Where(ra => ra.UserLoginId == userLogin.Id)
                .ToListAsync(ct);

            // Delete old assignments
            foreach (var assignment in existingAssignments)
            {
                await _roleAssignmentRepository.DeleteAsync(assignment, ct);
            }
                
            // Create new role assignments
            foreach (var role in roles)
            {
                await _roleAssignmentRepository.AddAsync(new UserRoleAssignment
                {
                    UserLoginId = userLogin.Id,
                    UserLoginRoleId = role.Id
                }, ct);
            }

            await _dbRepository.UpdateAsync(userLogin, ct);
        }
        else
        {
            var person = await _personDbRepository.GetByIdAsync(command.PersonId, ct) 
                         ?? throw new ArgumentNullException(nameof(command.PersonId));

            userLogin = new UserLogin
            {
                PersonId = command.PersonId,
                Tenant = command.Tenant,
                Username = person.Email.IsTruthy() && person.Email.IsActive.IsTruthy() 
                    ? person.Email.Address 
                    : $"{person.FullName.FirstName}.{person.FullName.LastName}",
                Password = BCrypt.Net.BCrypt.HashPassword("pancake"),
                UserRoles = roles.Select(role => new UserRoleAssignment
                {
                    UserLoginRoleId = role.Id
                }).ToList()
            };

            await _dbRepository.AddAsync(userLogin, ct);
        }

        return new ApiResponse();
    }
    
    private async Task<List<UserLoginRole>> GetOrCreateRoles(List<string> roleNames, CancellationToken ct)
    {
        var roles = new List<UserLoginRole>();

        foreach (var roleName in roleNames)
        {
            var role = await _roleRepository
                .Queryable()
                .FirstOrDefaultAsync(r => r.Name == roleName, ct);

            if (role == null)
            {
                role = new UserLoginRole(roleName);
                await _roleRepository.AddAsync(role, ct);
            }

            roles.Add(role);
        }

        return roles;
    }
}