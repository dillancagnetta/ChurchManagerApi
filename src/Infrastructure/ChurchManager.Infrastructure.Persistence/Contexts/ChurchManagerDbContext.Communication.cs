#region

using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communications;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Message> Message { get; set; }
        public DbSet<Communication> Communication { get; set; }
        public DbSet<CommunicationTemplate> CommunicationTemplate { get; set; }
    }
}
