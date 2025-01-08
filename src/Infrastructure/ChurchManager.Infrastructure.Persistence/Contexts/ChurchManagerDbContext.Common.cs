using ChurchManager.Domain.Features.History;
using CodeBoss.Jobs.Model;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<History> History { get; set; }
        public DbSet<ServiceJob> ServiceJobs { get; set; }
    }
}
