#region

using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Configurations
{
    public class FamilyConfiguration : IEntityTypeConfiguration<Family>
    {
        public void Configure(EntityTypeBuilder<Family> builder)
        {
            builder
                .Property(e => e.RecordStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new RecordStatus(v));

            builder.HasIndex(x => x.Name);
            
            builder
                .HasMany(f => f.FamilyMembers)
                .WithOne(p => p.Family)
                .HasForeignKey(p => p.FamilyId)
                .IsRequired(true); // false If FamilyId is nullable
        }
    }
}