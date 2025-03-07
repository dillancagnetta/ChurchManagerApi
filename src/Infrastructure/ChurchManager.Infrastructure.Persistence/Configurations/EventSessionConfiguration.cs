using ChurchManager.Domain.Features.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class SessionRegistrationConfiguration : IEntityTypeConfiguration<EventSessionRegistration>
{
    public void Configure(EntityTypeBuilder<EventSessionRegistration> builder)
    {
        // Relationships
        builder.HasOne(x => x.EventRegistration)
            .WithMany(x => x.SessionRegistrations)
            .HasForeignKey(x => x.EventRegistrationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.EventSession)
            .WithMany(x => x.SessionRegistrations)
            .HasForeignKey(x => x.EventSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => new { x.EventRegistrationId, x.EventSessionId })
            .IsUnique();  // Can't register for same session twice
        builder.HasIndex(x => x.PersonId);  // Person lookups
    }
}