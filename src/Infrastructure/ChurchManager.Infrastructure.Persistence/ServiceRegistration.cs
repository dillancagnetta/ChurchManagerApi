﻿using ChurchManager.Domain.Features.Churches.Repositories;
using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Domain.Features.Discipleship.Repositories;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.History;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Repositories;
using ChurchManager.Infrastructure.Persistence.Seeding;
using ChurchManager.Infrastructure.Persistence.Seeding.Development;
using ChurchManager.Infrastructure.Persistence.Seeding.Production;
using ChurchManager.Infrastructure.Persistence.Triggers;
using ChurchManager.Persistence.Shared;
using CodeBoss.AspNetCore.Startup;
using Convey;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            // Add to DI
            services.Configure<DbOptions>(configuration.GetSection(nameof(DbOptions)));        
            //services.AddDbContext<ChurchManagerDbContext>(options =>
            //   options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            //       x => x.MigrationsAssembly("ChurchManager.Infrastructure.Persistence")));
            services.AddDbContext<ChurchManagerDbContext>(options => 
                // Add Triggers
                options.UseTriggers(triggerOptions => {
                    triggerOptions.AddTrigger<PersonTrigger>();
                    triggerOptions.AddTrigger<FollowUpTrigger>();
                    triggerOptions.AddTrigger<GroupTrigger>();
                    triggerOptions.AddTrigger<GroupMemberTrigger>();
                    triggerOptions.AddTrigger<GroupMemberAttendanceTrigger>();
                    //triggerOptions.AddTrigger<SendEmailTrigger>();
            }));
            
            // Database Health Check 
            services
                .AddHealthChecks()
                .AddDbContextCheck<ChurchManagerDbContext>();

            services.AddScoped<IChurchManagerDbContext>(s => s.GetService<ChurchManagerDbContext>());
            services.AddScoped<DbContext>(s => s.GetService<ChurchManagerDbContext>());

            // Migrate database
            services.AddHostedService<DbTenantMigrationHostedService>();

            // Seeding: Switch this off in `appsettings.json`
            bool seedDatabaseEnabled = configuration.GetOptions<DbOptions>(nameof(DbOptions)).Seed;
            if (seedDatabaseEnabled)
            {
                services.AddInitializer<ChurchAttendanceTypeDbInitializer>();
                services.AddInitializer<DiscipleshipDbSeedInitializer>();

                if(environment.IsProduction())
                {
                    // SMALL DATA SET
                    /*services.AddInitializer<ChurchesDbSeedInitializer>();
                    services.AddInitializer<PeopleDbSeedInitializer>();
                    services.AddInitializer<GroupsDbSeedInitializer>();*/

                    // FAKE DATA
                    services.AddInitializer<ChurchesFakeDbSeedInitializer>();
                    services.AddInitializer<PeopleFakeDbSeedInitializer>();
                    services.AddInitializer<GroupsFakeDbSeedInitializer>();
                    services.AddInitializer<ChurchAttendanceFakeDbInitializer>();
                    services.AddInitializer<GroupAttendanceFakeDbSeedInitializer>();
                    services.AddInitializer<FollowUpFakeDbSeedInitializer>();
                    services.AddInitializer<MissionsFakeDbSeedInitializer>();
                }
                // Development / Test -  Seeding
                else
                {
                    /*services.AddInitializer<ChurchesDbSeedInitializer>();
                    services.AddInitializer<PeopleDbSeedInitializer>();
                    services.AddInitializer<GroupsDbSeedInitializer>();*/
                    services.AddInitializer<ChurchesFakeDbSeedInitializer>();
                    services.AddInitializer<PeopleFakeDbSeedInitializer>();
                    services.AddInitializer<GroupsFakeDbSeedInitializer>();
                    services.AddInitializer<ChurchAttendanceFakeDbInitializer>();
                    services.AddInitializer<GroupAttendanceFakeDbSeedInitializer>();
                    services.AddInitializer<FollowUpFakeDbSeedInitializer>();
                    services.AddInitializer<MissionsFakeDbSeedInitializer>();
                }
            }

            #region Repositories

            // TODO: scan and register these automatically
            services.AddScoped(typeof(IGenericDbRepository<>), typeof(GenericRepositoryBase<>));
            services.AddScoped<IGroupAttendanceDbRepository, GroupAttendanceDbRepository>();
            services.AddScoped<IChurchAttendanceDbRepository, ChurchAttendanceDbRepository>();
            services.AddScoped<IDiscipleshipStepDefinitionDbRepository, DiscipleshipDbRepository>();
            services.AddScoped<IGroupMemberDbRepository, GroupMemberDbRepository>();
            services.AddScoped<IGroupTypeRoleDbRepository, GroupTypeRoleDbRepository>();
            services.AddScoped<IPersonDbRepository, PersonDbRepository>();
            services.AddScoped<IGroupDbRepository, GroupDbRepository>();
            services.AddScoped<IGroupMemberAttendanceDbRepository, GroupMemberAttendanceDbRepository>();
            services.AddScoped<IHistoryDbRepository, HistoryDbRepository>();
            services.AddScoped<IPushSubscriptionsService, PushSubscriptionsService>();

            #endregion
        }
    }
}
