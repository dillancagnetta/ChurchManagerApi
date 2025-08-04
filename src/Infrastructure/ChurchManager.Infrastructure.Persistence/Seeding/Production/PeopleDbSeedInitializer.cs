#region

using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using CodeBoss.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Person = ChurchManager.Domain.Features.People.Person;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Seeding.Production
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class PeopleDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 2;
        private readonly IServiceScopeFactory _scopeFactory;
        private ChurchManagerDbContext _dbContext;
        private ITenant _tenant;

        public PeopleDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            _tenant = scope.ServiceProvider.GetRequiredService<ITenantsProvider<TenantConfiguration>>().Tenants().First();
            _dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await _dbContext.Person.AnyAsync())
            {
                await SeedMyDetails();
            }
        }

        private async Task SeedMyDetails()
       {
            var mainFamily = new Family {Name = "Main Family", Language = "English"};
            await _dbContext.SaveChangesAsync();

            // Add me as the first Person i.e. with Id 1
            var mainPerson = new Person
            {
                Family = mainFamily,
                AgeClassification = AgeClassification.Adult,
                RecordStatus = RecordStatus.Active,
                Gender = Gender.Unknown,
                PhotoUrl = null,
                ConnectionStatus = ConnectionStatus.Member,
                BaptismStatus = new Baptism {IsBaptised = true},
                ChurchId = 1,
                Email = new Email {Address = "admin@yahoo.com", IsActive = true},
                FullName = new FullName {FirstName = "Admin", LastName = "Admin"},
                //MaritalStatus = "Married",
                //AnniversaryDate = new DateTime(2013, 01, 22),
                UserLoginId = SeedingConstants.MainUserLogin,
                //BirthDate = new BirthDate {BirthDay = 6, BirthMonth = 11, BirthYear = 1981},
                ReceivedHolySpirit = true,
                Occupation = "Administrator",
                PhoneNumbers = new List<PhoneNumber>(1) {new(){CountryCode = "+27", Number = SeedingConstants.TestPhoneNumber.CleanPhoneNumber()}}
            };
            

            // Church Group Admin gets dynamic access to all churches in their group
            var permission = new EntityPermission
            {
                EntityType = "Church",
                IsDynamicScope = true,
                ScopeType = "ChurchGroup",
                ScopeId = 1, // churchGroupId
                CanView = true,
                CanEdit = true,
                CanDelete = true,
                IsSystem = true
            };
            var systemAdminRole = UserLoginRole.SystemAdminRole;
            var mainPersonUserLogin = new UserLogin
            {
                Id = Guid.Parse(SeedingConstants.MainUserLogin),
                Person = mainPerson,
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("pancake"),
                Tenant = _tenant.Name
            };
            mainPersonUserLogin.AddUserLoginRole(new UserRoleAssignment { UserLogin = mainPersonUserLogin, Role = systemAdminRole}); // System Admin

            await _dbContext.Person.AddAsync(mainPerson);
            await _dbContext.UserLoginRole.AddAsync(systemAdminRole);
            await _dbContext.UserLogin.AddAsync(mainPersonUserLogin);

            await _dbContext.SaveChangesAsync();
        }
    }
}
