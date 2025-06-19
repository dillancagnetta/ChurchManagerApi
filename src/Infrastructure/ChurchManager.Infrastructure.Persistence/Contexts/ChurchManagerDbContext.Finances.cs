#region

using ChurchManager.Domain.Features.Finances;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Benefactor> Benefactor { get; set; }
        public DbSet<Giving> Giving { get; set; }
        public DbSet<Partnership> Partnership { get; set; }
        public DbSet<Fund> Fund { get; set; }
    }
}
