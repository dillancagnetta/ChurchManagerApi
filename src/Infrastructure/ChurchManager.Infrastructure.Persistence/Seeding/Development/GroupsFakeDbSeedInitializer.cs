#region

using Bogus;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Ical.Net.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Seeding.Development
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class GroupsFakeDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 3;
        private readonly IServiceScopeFactory _scopeFactory;
        private ChurchManagerDbContext _dbContext;

        int personId = 2;

        // Cell Group Type
        private readonly GroupType _sectionGroupType = new() { Name = "Section", Description = "Group Section", IconCssClass = "heroicons_outline:folder", TakesAttendance = false, IsSystem = true};
        private readonly GroupType _cellGroupType = new() { Name = SeedingConstants.CellGroupType, Description = "Cell Ministry", IconCssClass = "heroicons_outline:squares-2x2", IsSystem = true };
        private readonly GroupType _eventsGroupType = new() { Name = "Events", Description = "Event Registration", IconCssClass = "heroicons_outline:calendar", IsSystem = true };
        private readonly GroupType _communicationsGroupType = new() { Name = "Communications", Description = "Storing lists of people to communicate to", 
            GroupTerm = "List", GroupMemberTerm = "Recipient", IconCssClass = "heroicons_outline:chat-bubble-left", IsSystem = true, TakesAttendance = false };

        private GroupTypeRole _cellLeaderRole;
        private GroupTypeRole _cellAssistantRole;
        private GroupTypeRole _groupMemberRole;
        private Church _church;

        private static readonly CalendarSerializer CalendarSerializer = new();

        public GroupsFakeDbSeedInitializer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();
            
            _church = _dbContext.Church.AsNoTracking().OrderBy(x => x.Id).First();

            if (!_dbContext.GroupType.Any())
            {
                await _dbContext.GroupType.AddAsync(_sectionGroupType);
                await _dbContext.GroupType.AddAsync(_cellGroupType);
                await _dbContext.GroupType.AddAsync(_eventsGroupType);
                await _dbContext.GroupType.AddAsync(_communicationsGroupType);
                await _dbContext.SaveChangesAsync();
            }

            if (!await _dbContext.GroupTypeRole.AnyAsync())
            {
                _cellLeaderRole = new GroupTypeRole
                {
                    Name = "Leader", Description = "Cell Leader", IsLeader = true,
                    CanView = true, CanEdit = true, CanManageMembers = true,
                    GroupType = _cellGroupType
                };
                _cellAssistantRole = new GroupTypeRole
                    { Name = "Assistant", Description = "Assistant Leader", GroupType = _cellGroupType, IsLeader = true };
                _groupMemberRole = new GroupTypeRole { Name = "Member", Description = "Group Member"};

                _groupMemberRole.GroupType = _cellGroupType;
                await _dbContext.GroupTypeRole.AddAsync(_cellLeaderRole);
                await _dbContext.GroupTypeRole.AddAsync(_cellAssistantRole);
                await _dbContext.GroupTypeRole.AddAsync(_groupMemberRole);
                
                // Communications
                _groupMemberRole.GroupType = _communicationsGroupType;
                await _dbContext.GroupTypeRole.AddAsync(_groupMemberRole);
                // Regisrations
                var registrantRole = new GroupTypeRole
                {
                    Name = "Registrant", Description = "Event Registrant",
                    GroupType = _eventsGroupType
                };
                await _dbContext.GroupTypeRole.AddAsync(registrantRole);

                await _dbContext.SaveChangesAsync();
            }

            if (!await _dbContext.Group.AnyAsync())
            {
                // Cell Groups Section
                var cellSectionParentGroup = new Group
                {
                    GroupType = _sectionGroupType,
                    Name = "Cell Groups",
                    Description = "Grouping section for cell groups",
                    CreatedDate = DateTime.UtcNow,
                    ChurchId = _church.Id
                };

                // await SeedMyGroups();
                await _dbContext.Group.AddRangeAsync(GenerateGroups(2, cellSectionParentGroup, groupLeaderPersonId: 1));
                await _dbContext.Group.AddRangeAsync(GenerateGroups(2, cellSectionParentGroup));
                await _dbContext.Group.AddRangeAsync(GenerateGroups(2, cellSectionParentGroup));
                await _dbContext.Group.AddRangeAsync(GenerateGroups(2, cellSectionParentGroup));
                await _dbContext.Group.AddRangeAsync(GenerateGroups(2, cellSectionParentGroup));
                await _dbContext.SaveChangesAsync();
                
                // Event Registration Groups
                var eventRegistrationGroup = new Group
                {
                    GroupType = _eventsGroupType,
                    Name = "Event Registration Group",
                    Description = "Event Registration Group",
                    CreatedDate = DateTime.UtcNow,
                };
                await _dbContext.Group.AddAsync(eventRegistrationGroup);
                await _dbContext.SaveChangesAsync();
                
                // Communication List Groups
                var communicationsSectionParentGroup = new Group
                {
                    GroupType = _communicationsGroupType,
                    Name = "Communication Lists",
                    Description = "Grouping section for communication lists",
                    CreatedDate = DateTime.UtcNow
                };
                var communicationsMembersList = new Group
                {
                    GroupType = _communicationsGroupType,
                    Name = "Members",
                    Description = "Church members communication list",
                    CreatedDate = DateTime.UtcNow,
                    ChurchId = _church.Id,
                    ParentGroup = communicationsSectionParentGroup
                };
                var communicationsParentsList = new Group
                {
                    GroupType = _communicationsGroupType,
                    Name = "Parents of Children",
                    Description = "Parents of Children communication list",
                    CreatedDate = DateTime.UtcNow,
                    ChurchId = _church.Id,
                    ParentGroup = communicationsSectionParentGroup
                };
                await _dbContext.Group.AddRangeAsync(communicationsSectionParentGroup, communicationsMembersList, communicationsParentsList);
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task SeedMyGroups()
        {
            if(!await _dbContext.Group.AnyAsync())
            {
                var faker = new Faker();
                var random = new Random();

                var cellLeader = new Faker<GroupMember>()
                    .RuleFor(u => u.PersonId, f => 1)
                    .RuleFor(u => u.GroupRoleId, f => 1);

                var cellMember = new Faker<GroupMember>()
                .RuleFor(u => u.PersonId, f => personId++)
                .RuleFor(u => u.GroupRoleId, f => 2);
                
                for(int i = 0; i < 4; i++)
                {
                    var cellGroupMembers = cellMember.Generate(random.Next(1, 30));
                    cellGroupMembers.Add(cellLeader);

                    _dbContext.Group.Add(new Group
                    {
                        Name = faker.Address.City() + " Cell Group",
                        Description = faker.Address.City() + " Cell Group",
                        GroupType = _cellGroupType,
                        ChurchId = _church.Id,
                        Members = cellGroupMembers,
                        StartDate = DateTimeOffset.UtcNow,
                        IsOnline = i % 2 == 0,
                        Address = faker.Address.FullAddress(),
                    });
                }

                await _dbContext.SaveChangesAsync();
            }
        }

        private IList<Group> GenerateGroups(int count, Group parentGroup, int? groupLeaderPersonId = null, bool generateChildren = true, int level = 0)
        {
            var faker = new Faker();
            var random = new Random();

            IList<Group> groups = new List<Group>(count);
            for(int i = 0; i < count; i++)
            {
                var fakeName =  $"{ faker.Address.City()} Cell";
                var group = new Group
                {
                    Name = parentGroup.Name == "Cell Groups" ? fakeName : $"{ parentGroup.Name} - {level + i}",
                    Description = fakeName,
                    GroupType = _cellGroupType,
                    ChurchId = _church.Id,
                    Members = GenerateGroupMembers(groupLeaderPersonId),
                    StartDate = DateTimeOffset.UtcNow,
                    IsOnline = i % 2 == 0,
                    Address = faker.Address.FullAddress(),
                    Schedule = parentGroup.Name == "Cell Groups" ? GenerateSchedule() : null,
                    ParentGroup = parentGroup
                };

                // Generate sub groups
                if (generateChildren)
                {
                    level++;
                    var children = GenerateGroups(random.Next(0, 4), group, null, false, level);
                    group.Groups = children;
                }

                groups.Add(group);
            }

            return groups;
        }

        private Schedule GenerateSchedule()
        {
            var calendar = InetCalendarHelper.CalendarWithWeeklyRecurrence(
                DateOnly.FromDateTime(DateTime.UtcNow), null,
                new TimeOnly(18, 0, 0), new [] {DayOfWeek.Thursday});

            return new Schedule
            {
                WeeklyDayOfWeek = DayOfWeek.Thursday,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                iCalendarContent = CalendarSerializer.SerializeToString(calendar),
            };
        }

        private IList<GroupMember> GenerateGroupMembers(int? groupLeaderPersonId = null)
        {
            var random = new Random();

            var totalPeopleInDb = _dbContext.Person.Count();

            var cellLeader = new Faker<GroupMember>()
                .RuleFor(u => u.PersonId, f => groupLeaderPersonId ?? random.Next(2, totalPeopleInDb / 2)) // The personIds generated
                .RuleFor(u => u.GroupRoleId, f => _cellLeaderRole.Id);

            var cellMember = new Faker<GroupMember>()
                .RuleFor(u => u.PersonId, f => random.Next((totalPeopleInDb / 2)+1, totalPeopleInDb))
                .RuleFor(u => u.GroupRoleId, f => _groupMemberRole.Id);

            var cellGroupMembers = cellMember.Generate(random.Next(1, 16));

            cellGroupMembers.Add(cellLeader);

            return cellGroupMembers;
        }
    }
}
