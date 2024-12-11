using ChurchManager.Domain;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations
{
    public class ChurchConfiguration : IEntityTypeConfiguration<Church>
    {
        public void Configure(EntityTypeBuilder<Church> builder)
        {
            builder
                .Property(e => e.RecordStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new RecordStatus(v));
            
            builder
                .HasOne(cg => cg.LeaderPerson)
                .WithOne()
                .HasForeignKey<Church>(cg => cg.LeaderPersonId)
                .IsRequired(false);
        }
        
        public class ChurchGroupConfiguration : IEntityTypeConfiguration<ChurchGroup>
        {
            public void Configure(EntityTypeBuilder<ChurchGroup> builder)
            {
                builder
                    .Property(e => e.RecordStatus)
                    .HasConversion(
                        v => v.ToString(),
                        v => new RecordStatus(v));
            
                builder
                    .HasOne(cg => cg.LeaderPerson)
                    .WithOne()
                    .HasForeignKey<ChurchGroup>(cg => cg.LeaderPersonId)
                    .IsRequired(false);
            }
        }
    }
}