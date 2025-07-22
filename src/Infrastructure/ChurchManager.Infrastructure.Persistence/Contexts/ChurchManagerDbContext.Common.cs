#region

using ChurchManager.Domain.Features.History;
using ChurchManager.Domain.Features.Settings;
using CodeBoss.Jobs.Model;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<History> History { get; set; }
        public DbSet<ServiceJob> ServiceJobs { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
