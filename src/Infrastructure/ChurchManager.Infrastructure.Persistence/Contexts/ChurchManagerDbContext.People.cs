﻿#region

using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.People;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Family> Family { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<OnlineUser> OnlineUser { get; set; }
        public DbSet<PushDevice> PushDevice { get; set; }
        public DbSet<FollowUp> FollowUp { get; set; }

    }
}
