﻿#region

using Bogus;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development
{
    public class ChurchAttendanceFakeDbInitializer : IInitializer
    {
        public int OrderNumber => 5;
        
        private readonly IServiceScopeFactory _scopeFactory;

        public ChurchAttendanceFakeDbInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await dbContext.ChurchAttendance.AnyAsync())
            {
                var attendances = new List<ChurchAttendance>(10000); 
                for (int churchId = 1; churchId < 10; churchId++)
                {
                    attendances.AddRange(GenerateChurchAttendances(1000, churchId));
                    await dbContext.ChurchAttendance.AddRangeAsync(attendances);
                }

                await dbContext.SaveChangesAsync();
            }
        }

        private IList<ChurchAttendance> GenerateChurchAttendances(int count, int churchId)
        {
            var faker = new Faker();
            var random = new Random();

            var churchAttendances = new List<ChurchAttendance>(count);
            
            for(int i = 0; i < count; i++)
            {
                var attendance = new
                {
                    attendancedate = faker.Date.Between(new DateTime(2018, 01, 01), DateTime.UtcNow),
                    males = random.Next(0, 500),
                    females = random.Next(0, 500),
                    children = random.Next(0, 200),
                    firsttimers = random.Next(10, 100),
                };

                churchAttendances.Add( new ChurchAttendance
                {
                    ChurchAttendanceTypeId = random.Next(1, 3),  // will generate 1 to 2 only
                    ChurchId = churchId,
                    AttendanceDate = attendance.attendancedate,
                    AttendanceCount = attendance.males + attendance.females + attendance.children,
                    ChildrenCount = attendance.children,
                    FemalesCount = attendance.females,
                    MalesCount = attendance.males,
                    FirstTimerCount = attendance.firsttimers,
                    // Max half the first timers will be born again
                    NewConvertCount = random.Next(0, (int)Math.Round((decimal)attendance.firsttimers/2)),
                    ReceivedHolySpiritCount = random.Next(0, (int)Math.Round((decimal)attendance.firsttimers / 2))
                });
            }

            return churchAttendances;
        }
    }
}
