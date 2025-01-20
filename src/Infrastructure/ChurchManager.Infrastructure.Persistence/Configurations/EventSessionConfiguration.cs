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

        builder.HasOne(x => x.SessionSchedule)
            .WithMany(x => x.SessionRegistrations)
            .HasForeignKey(x => x.SessionScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => new { x.EventRegistrationId, x.SessionScheduleId })
            .IsUnique();  // Can't register for same session twice
    }
}

public class SessionScheduleConfiguration : IEntityTypeConfiguration<EventSessionSchedule>
{
    public void Configure(EntityTypeBuilder<EventSessionSchedule> builder)
    {

        // Relationships
        builder.HasOne(x => x.EventSession)
            .WithMany(x => x.SessionSchedules)
            .HasForeignKey(x => x.EventSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        

        builder.HasMany(x => x.SessionRegistrations)
            .WithOne(x => x.SessionSchedule)
            .HasForeignKey(x => x.SessionScheduleId);

        // Index for performance
        builder.HasIndex(x => x.StartDateTime);
        builder.HasIndex(x => x.EventSessionId);    }
}