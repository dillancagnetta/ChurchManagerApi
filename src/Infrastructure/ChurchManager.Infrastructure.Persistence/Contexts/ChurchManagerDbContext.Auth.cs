#region

using ChurchManager.Domain.Common;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<UserLogin> UserLogin { get; set; }
    }
}
