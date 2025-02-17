#region

using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Churches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Configurations;

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
    
    /*public class ChurchAttendanceConfiguration : IEntityTypeConfiguration<ChurchAttendance>
    {
        public void Configure(EntityTypeBuilder<ChurchAttendance> builder)
        {
            builder
                .Property(e => e.RecordStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => new RecordStatus(v));
            
            builder
                .HasOne(cg => cg.ChurchAttendanceType)
                .WithOne()
                .HasForeignKey<ChurchAttendance>(cg => cg.ChurchAttendanceTypeId)
                .IsRequired(true);
        }
    }*/
}