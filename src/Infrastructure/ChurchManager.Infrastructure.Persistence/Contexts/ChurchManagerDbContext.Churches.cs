#region

using ChurchManager.Domain.Features.Churches;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Church> Church { get; set; }
        public DbSet<ChurchAttendanceType> ChurchAttendanceType { get; set; }
        public DbSet<ChurchAttendance> ChurchAttendance { get; set; }
    }
}
