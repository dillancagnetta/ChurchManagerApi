using ChurchManager.Domain.Features.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class ConnectionStatusHistoryConfiguration: IEntityTypeConfiguration<ConnectionStatusHistory>
{
    public void Configure(EntityTypeBuilder<ConnectionStatusHistory> builder)
    {
        builder
            .Property(e => e.RecordStatus)
            .HasRecordStatus();
        
        builder.HasIndex(o => o.PersonId);
        builder.HasIndex(o => o.ConnectionStatusTypeId);
        builder.HasIndex(o => new { o.PersonId, o.ConnectionStatusTypeId });
        
        // Configure the relationship with person
        // If person is deleted - all history will be deleted
        builder
            .HasOne(h => h.Person)
            .WithMany(p => p.ConnectionStatusHistory)
            .HasForeignKey(h => h.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Configure the relationship with connection status type
        builder
            .HasOne(h => h.ConnectionStatusType)
            .WithMany()
            .HasForeignKey(h => h.ConnectionStatusTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ConnectionStatusTypeConfiguration: IEntityTypeConfiguration<ConnectionStatusType>
{
    public void Configure(EntityTypeBuilder<ConnectionStatusType> builder)
    {
        builder
            .Property(e => e.Name)
            .HasEnumerationConversion<ConnectionStatus>();
        
        builder.HasIndex(o => o.Name);
    }
}