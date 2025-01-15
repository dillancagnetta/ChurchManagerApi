using ChurchManager.Domain.Features;
using ChurchManager.Domain.Features.Communication;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Message> Message { get; set; }
    }
}
