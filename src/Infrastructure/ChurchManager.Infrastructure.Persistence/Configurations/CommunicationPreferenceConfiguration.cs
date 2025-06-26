using ChurchManager.Domain.Features.Communications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class CommunicationPreferenceTypeConfiguration: IEntityTypeConfiguration<CommunicationPreferenceType>
{
    public void Configure(EntityTypeBuilder<CommunicationPreferenceType> builder)
    {
        builder.ToTable(nameof(CommunicationPreferenceType), "Communications");

        builder.HasIndex(e => e.Name).IsUnique();
        
        builder
            .Property(e => e.DefaultCommunicationType)
            .HasEnumerationConversion<CommunicationType>();
    }
}

public class CommunicationPreferenceConfiguration: IEntityTypeConfiguration<CommunicationPreference>
{
    public void Configure(EntityTypeBuilder<CommunicationPreference> builder)
    {
        builder.ToTable(nameof(CommunicationPreference), "Communications");

        builder.HasIndex(e => new { e.PersonId, e.PreferenceTypeId, e.CommunicationType })
            .IsUnique();
        
        builder
            .Property(e => e.CommunicationType)
            .HasEnumerationConversion<CommunicationType>();
            
        builder.HasOne(d => d.PreferenceType)
            .WithMany(p => p.Preferences)
            .HasForeignKey(d => d.PreferenceTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}