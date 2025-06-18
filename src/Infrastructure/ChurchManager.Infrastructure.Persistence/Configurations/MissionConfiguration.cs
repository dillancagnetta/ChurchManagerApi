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
            
            // Configure the relationship with Church
            // If Church is deleted - all groups will be deleted
            builder
                .HasOne(p => p.Church)
                .WithMany()
                .HasForeignKey(p => p.ChurchId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasOne(p => p.Person)
                .WithMany()
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasOne(p => p.Group)
                .WithMany()
                .HasForeignKey(p => p.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
