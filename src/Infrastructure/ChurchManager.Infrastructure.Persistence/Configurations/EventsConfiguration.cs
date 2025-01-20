using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Features.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class EventsConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder
            .Property(e => e.ApprovalStatus)
            .HasConversion(
                v => v.ToString(),
                v => new ApprovalStatus(v));
        
        builder
            .Property(e => e.RecordStatus)
            .HasRecordStatus();

        builder.OwnsOne(x => x.Review);
        
        builder.HasOne(r => r.ContactPerson)
            .WithMany()
            .HasForeignKey(r => r.ContactPersonId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.Schedule)
            .WithMany()
            .HasForeignKey(r => r.ScheduleId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
        
        /* Optional Properties */
        
        builder.HasOne(r => r.Church)
            .WithMany()
            .HasForeignKey(r => r.ChurchId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.ChurchGroup)
            .WithMany()
            .HasForeignKey(r => r.ChurchGroupId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.EventRegistrationGroup)
            .WithMany()
            .HasForeignKey(r => r.EventRegistrationGroupId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.ChildCareGroup)
            .WithMany()
            .HasForeignKey(r => r.ChildCareGroupId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
        
        /* Optional Properties */
    }
}

public class EventTypeConfiguration : IEntityTypeConfiguration<EventType>
{
    public void Configure(EntityTypeBuilder<EventType> builder)
    {
        /*builder
            .Property(e => e.AgeClassification)
            .HasConversion(
                v => v.ToString(),
                v => new AgeClassification(v));*/

        builder
            .Property(e => e.AgeClassification)
            .HasEnumerationConversion<AgeClassification>();

        builder
            .Property(c => c.RecordStatus)
            .HasRecordStatus();
        
        builder.OwnsOne(x => x.ChildCare);
       
        /* Optional Properties */
        
        builder.HasOne(r => r.DefaultGroupType)
            .WithMany()
            .HasForeignKey(r => r.DefaultGroupTypeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        /* Optional Properties */
    }
}

public class EventRegistrationConfiguration : IEntityTypeConfiguration<EventRegistration>
{
    public void Configure(EntityTypeBuilder<EventRegistration> builder)
    {
        builder
            .Property(e => e.Status)
            .HasEnumerationConversion<RegistrationStatus>();

        builder
            .Property(c => c.RecordStatus)
            .HasRecordStatus();
        
        // Relationships
        builder.HasOne(x => x.Event)
            .WithMany(x => x.Registrations)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
        

       
        builder.HasMany(x => x.SessionRegistrations)
            .WithOne(x => x.EventRegistration)
            .HasForeignKey(x => x.EventRegistrationId);
        
        /* Optional Properties */
        
        builder.HasOne(r => r.RegisteredByPerson)
            .WithMany()
            .HasForeignKey(r => r.RegisteredByPersonId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(x => x.Group)
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Restrict);
        
        /* Optional Properties */
        
        // Indexes
        builder.HasIndex(x => new { x.EventId, x.PersonId })
            .IsUnique();  // Person can only register once for an event
    }
}

public class EventSessionConfiguration : IEntityTypeConfiguration<EventSession>
{
    public void Configure(EntityTypeBuilder<EventSession> builder)
    {
        builder.Property(x => x.SessionOrder)
            .IsRequired();

        // Relationships
        builder.HasOne(x => x.Event)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SessionSchedules)
            .WithOne(x => x.EventSession)
            .HasForeignKey(x => x.EventSessionId);

        builder.HasMany(x => x.SessionRegistrations)
            .WithOne()
            .HasForeignKey(x => x.SessionScheduleId);
    }
}

