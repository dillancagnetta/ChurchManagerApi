#region

using ChurchManager.Domain.Features.Events;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Event> Event { get; set; }
        public DbSet<EventType> EventType { get; set; }
    }
}
