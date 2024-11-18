using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.History;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<History> History { get; set; }
    }
}
