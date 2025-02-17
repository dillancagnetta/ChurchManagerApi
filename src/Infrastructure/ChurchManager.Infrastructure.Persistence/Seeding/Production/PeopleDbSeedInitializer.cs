﻿#region

using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
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
        public int OrderNumber { get; } = 1;
        private readonly IServiceScopeFactory _scopeFactory;
        private ChurchManagerDbContext _dbContext;
        private ITenant _tenant;

        public PeopleDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            _tenant = scope.ServiceProvider.GetRequiredService<ITenantProvider>().Tenants().FirstOrDefault();
            _dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await _dbContext.Person.AnyAsync())
            {
                await SeedMyDetails();
            }
        }

        private async Task SeedMyDetails()
       {
            var cagnettaFamily = new Family {Name = "Cagnetta Family", Language = "English"};
            await _dbContext.SaveChangesAsync();

            // Add me as the first Person i.e. with Id 1
            var dillan = new Person
            {
                Family = cagnettaFamily,
                AgeClassification = AgeClassification.Adult,
                RecordStatus = RecordStatus.Active,
                Gender = Gender.Male,
                PhotoUrl = "https://secure.gravatar.com/avatar/6fdc48b6ec4d95f2fd682fc2982eb01b",
                ConnectionStatus = ConnectionStatus.Member,
                BaptismStatus = new Baptism {IsBaptised = true},
                ChurchId = 1,
                Email = new Email {Address = "dillancagnetta@yahoo.com", IsActive = true},
                FullName = new FullName {FirstName = "Dillan", LastName = "Cagnetta"},
                MaritalStatus = "Married",
                AnniversaryDate = new DateTime(2013, 01, 22),
                UserLoginId = "08925ade-9249-476b-8787-b3dd8f5dbc13",
                BirthDate = new BirthDate {BirthDay = 6, BirthMonth = 11, BirthYear = 1981},
                ReceivedHolySpirit = true,
                Occupation = "Software developer",
                PhoneNumbers = new List<PhoneNumber>(1) {new(){CountryCode = "+27", Number = "0737378631"}}
            };

            var danielle = new Person
            {
                Family = cagnettaFamily,
                AgeClassification = AgeClassification.Adult,
                RecordStatus = RecordStatus.Active,
                Gender = Gender.Female,
                PhotoUrl = null,
                ConnectionStatus = ConnectionStatus.Member,
                BaptismStatus = new Baptism { IsBaptised = true },
                ChurchId = 1,
                Email = new Email { Address = "danielle@yahoo.com", IsActive = true },
                FullName = new FullName { FirstName = "Danielle", LastName = "Cagnetta" },
                MaritalStatus = "Married",
                AnniversaryDate = new DateTime(2013, 01, 22),
                BirthDate = new BirthDate { BirthDay = 13, BirthMonth = 03, BirthYear = 1980 },
                ReceivedHolySpirit = true,
                Occupation = "Church Right hand",
                PhoneNumbers = new List<PhoneNumber>()
            };
            var david = new Person
            {
                Family = cagnettaFamily,
                AgeClassification = AgeClassification.Child,
                RecordStatus = RecordStatus.Active,
                Gender = Gender.Male,
                PhotoUrl = null,
                ConnectionStatus = ConnectionStatus.Member,
                BaptismStatus = new Baptism { IsBaptised = false },
                ChurchId = 1,
                FullName = new FullName { FirstName = "David", LastName = "Cagnetta" },
                BirthDate = new BirthDate { BirthDay = 06, BirthMonth = 07, BirthYear = 2017 },
                ReceivedHolySpirit = false,
            };

            var daniel = new Person
            {
                Family = cagnettaFamily,
                AgeClassification = AgeClassification.Child,
                RecordStatus = RecordStatus.Active,
                Gender = Gender.Male,
                PhotoUrl = null,
                ConnectionStatus = ConnectionStatus.Member,
                BaptismStatus = new Baptism { IsBaptised = true },
                ChurchId = 1,
                FullName = new FullName { FirstName = "Daniel", LastName = "Cagnetta" },
                BirthDate = new BirthDate { BirthDay = 28, BirthMonth = 06, BirthYear = 2013 },
                ReceivedHolySpirit = true,
            };

            var dillanUserLogin = new UserLogin
            {
                Id = Guid.Parse("08925ade-9249-476b-8787-b3dd8f5dbc13"),
                Person = dillan,
                Username = "dillan",
                Password = BCrypt.Net.BCrypt.HashPassword("81118599"),
                Roles = new List<string>{ "Admin" },
                Tenant = _tenant.Name
            };

            await _dbContext.Person.AddAsync(dillan);
            await _dbContext.Person.AddAsync(danielle);
            await _dbContext.Person.AddAsync(david);
            await _dbContext.Person.AddAsync(daniel);

            await _dbContext.UserLogin.AddAsync(dillanUserLogin);

            await _dbContext.SaveChangesAsync();
        }
    }
}
