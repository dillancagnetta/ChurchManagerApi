using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development
{
    public class GroupMemberAttendanceFakeDbSeedInitializer : IInitializer
    {
        public int OrderNumber => 99;

        private readonly IServiceScopeFactory _scopeFactory;

        public GroupMemberAttendanceFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) =>
            _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();

            if (!await dbContext.GroupMemberAttendance.AnyAsync())
            {
                var groups = dbContext.Group
                    .Include(x => x.Members)
                    .Where(x => x.GroupTypeId == 2) // Cells Groups
                    .AsQueryable().Take(10)
                    .AsNoTracking();

                var faker = new Faker();
                var random = new Random();

                var attendees = new List<GroupMemberAttendance>();
                var attendances = new List<GroupAttendance>();

                DateTime from = DateTime.Today.AddYears(-2); // Start from 2 years ago
                // Create weekly meetings for each group
                foreach (var group in groups)
                {
                    // Generate Thursday meetings
                    attendances.AddRange(GetSeedData(faker, group, from, DayOfWeek.Thursday));
                }
                
                await dbContext.GroupAttendance.AddRangeAsync(attendances);
                await dbContext.SaveChangesAsync();
            }
        }

        public List<GroupAttendance> GetSeedData(Faker faker, Group group, DateTime from, DayOfWeek dayOfWeek)
        {
            var attendees = new List<GroupMemberAttendance>();
            var attendances = new List<GroupAttendance>();
            var attendedMembers = new HashSet<(int GroupId, int MemberId)>();
            
            var members = group.Members.ToList();
            
            foreach (var meetingDate in from.GetWeeklyDates(dayOfWeek))
            {
                // Keep track of members who attended this meeting
                var meetingAttendees = new List<GroupMemberAttendance>(members.Count());
                foreach (var member in members)
                {
                    var didAttend = faker.Random.Bool();
                    var isFirstTime = !attendedMembers.Contains((group.Id, member.Id));
                            
                    if (didAttend)
                    {
                        attendedMembers.Add((group.Id, member.Id));
                    }

                    // Generate attendance for this member
                    var attendance = new GroupMemberAttendance
                    {
                        GroupId = group.Id,
                        GroupMemberId = member.Id,
                        AttendanceDate = meetingDate,
                        DidAttend = didAttend,
                        IsFirstTime = isFirstTime && didAttend,
                        // Received Holy Spirit on the first time attendance
                        ReceivedHolySpirit = isFirstTime && didAttend && faker.Random.Bool(0.1f),
                        // New Convert on the first time attendance
                        IsNewConvert = isFirstTime && didAttend && faker.Random.Bool(0.2f),
                    };
                    
                    meetingAttendees.Add(attendance);
                }
                
                // Generate attendance for this group meeting
                var groupAttendance = new GroupAttendance
                {
                    GroupId = group.Id,
                    AttendanceDate = meetingDate,
                    AttendanceCount = meetingAttendees.Count(x => x.DidAttend.HasValue && x.DidAttend.Value),
                    FirstTimerCount = meetingAttendees.Count(x => x.IsFirstTime.HasValue && x.IsFirstTime.Value),
                    // Max half the first timers will be born again
                    NewConvertCount = meetingAttendees.Count(x => x.IsNewConvert.HasValue && x.IsNewConvert.Value),
                    ReceivedHolySpiritCount = meetingAttendees.Count(x => x.ReceivedHolySpirit.HasValue && x.ReceivedHolySpirit.Value),
                    Attendees = meetingAttendees
                };
                    
                attendances.Add(groupAttendance);
            }
            
            return attendances;
        }
    }
}