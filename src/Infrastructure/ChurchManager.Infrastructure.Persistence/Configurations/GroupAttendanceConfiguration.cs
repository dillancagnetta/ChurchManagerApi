#region

using ChurchManager.Domain.Features.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Configurations
{
    public class GroupAttendanceConfiguration : IEntityTypeConfiguration<GroupAttendance>
    {
        public void Configure(EntityTypeBuilder<GroupAttendance> builder)
        {
            builder.OwnsOne(x => x.Offering);
            builder.HasMany(b => b.Attendees).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
