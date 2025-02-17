#region

using ChurchManager.Domain.Features.Missions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Configurations
{
    public class MissionConfiguration : IEntityTypeConfiguration<Mission>
    {
        public void Configure(EntityTypeBuilder<Mission> builder)
        {
            builder.OwnsOne(x => x.Offering);
            
            // Indexes
            builder.HasIndex(x => x.Name);  // name lookups
            builder.HasIndex(x => new { x.Name, x.RecordStatus });  // Active role lookups by name
            builder.HasIndex(x => x.RecordStatus);  // Status filtering
            builder.HasIndex(x => x.Type);  // Type filtering
        }
    }
}
