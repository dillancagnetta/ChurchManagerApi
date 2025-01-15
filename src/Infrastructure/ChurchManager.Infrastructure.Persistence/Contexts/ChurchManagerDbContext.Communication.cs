#region

using ChurchManager.Domain.Features.Communication;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Message> Message { get; set; }
    }
}
