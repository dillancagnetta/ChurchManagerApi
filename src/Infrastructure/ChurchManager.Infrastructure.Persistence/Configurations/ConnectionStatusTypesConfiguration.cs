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