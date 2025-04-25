using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.MultiTenant;
using DotLiquid.Util;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.UserLogins.Commands.AddUserLogin;

public record AddOrUpdateUserLoginCommand : IRequest<ApiResponse>
{
    public int PersonId { get; set; }
    public string? Password { get; set; }
    public List<int> UserLoginRoleIds { get; set; } = new(0); // RoleIds
}

public class AddUserLoginHandler : IRequestHandler<AddOrUpdateUserLoginCommand, ApiResponse>
{
    private readonly IGenericDbRepository<UserLogin> _dbRepository;
    private readonly IGenericDbRepository<UserLoginRole> _roleRepository;
    private readonly IGenericDbRepository<UserRoleAssignment> _roleAssignmentRepository;
    private readonly IPersonDbRepository _personDbRepository;
    private readonly ITenantCurrentUser _tenantCurrentUser;

    public AddUserLoginHandler(
        IGenericDbRepository<UserLogin> dbRepository,
        IGenericDbRepository<UserLoginRole> roleRepository,
        IGenericDbRepository<UserRoleAssignment> roleAssignmentRepository,
        IPersonDbRepository personDbRepository, 
        ITenantCurrentUser tenantCurrentUser)
    {
        _dbRepository = dbRepository;
        _roleRepository = roleRepository;
        _roleAssignmentRepository = roleAssignmentRepository;
        _personDbRepository = personDbRepository;
        _tenantCurrentUser = tenantCurrentUser;
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
        //var roles = await GetOrCreateRoles(command.UserLoginRoleIds, ct);

        if (userLogin is not null)
        {
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
            foreach (var roleId in command.UserLoginRoleIds)
            {
                await _roleAssignmentRepository.AddAsync(new UserRoleAssignment
                {
                    UserLoginId = userLogin.Id,
                    UserLoginRoleId = roleId
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
                Tenant = _tenantCurrentUser.Tenant,
                Username = person.Email.IsTruthy() && person.Email.IsActive.IsTruthy() 
                    ? person.Email.Address 
                    : $"{person.FullName.FirstName}.{person.FullName.LastName}",
                Password = BCrypt.Net.BCrypt.HashPassword(command.Password),
                UserRoles = command.UserLoginRoleIds.Select(roleId => new UserRoleAssignment
                {
                    UserLoginRoleId = roleId
                }).ToList()
            };

            await _dbRepository.AddAsync(userLogin, ct);
        }

        return new ApiResponse();
    }
    
    private async Task<List<int>> GetOrCreateRoles(List<int> roleIds, CancellationToken ct)
    {
        var roles = new List<int>();

        foreach (var roleId in roleIds)
        {
            var role = await _roleRepository
                .Queryable()
                .AsNoTracking()
                .Where(r => r.Id == roleId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(ct)
               ;

            if (role != 0) roles.Add(role);
  
            /*if (role == null)
            {
                role = new UserLoginRole(roleName);
                await _roleRepository.AddAsync(role, ct);
            }*/
        }

        return roles;
    }
}