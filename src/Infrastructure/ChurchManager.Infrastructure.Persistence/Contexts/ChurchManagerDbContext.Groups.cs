﻿#region

using ChurchManager.Domain.Features.Groups;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupType> GroupType { get; set; }
        public DbSet<GroupAttendance> GroupAttendance { get; set; }
        public DbSet<GroupMember> GroupMember { get; set; }
        public DbSet<GroupMemberAttendance> GroupMemberAttendance { get; set; }
        public DbSet<GroupTypeRole> GroupTypeRole { get; set; }
    }
}
