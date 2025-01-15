#region

using ChurchManager.Domain.Features.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class HistoryConfiguration: IEntityTypeConfiguration<History>
{
    public void Configure(EntityTypeBuilder<History> builder)
    {
        builder
            .Property(e => e.ChangeType)
            .HasConversion(
                v => v.ToString(),
                v => new HistoryChangeType(v) );
        
        builder.HasIndex(o => o.EntityType);
        builder.HasIndex(o => o.EntityId);
        builder.HasIndex(o => o.RelatedEntityId);
        builder.HasIndex(o => o.RelatedEntityType);
    }
}