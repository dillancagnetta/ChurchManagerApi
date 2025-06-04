#region

using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder
            .Property(e => e.RecordStatus)
            .HasConversion(
                v => v.ToString(),
                v => new RecordStatus(v) );

        builder
            .Property(e => e.ConnectionStatus)
            .HasConversion(
                v => v.ToString(),
                v => new ConnectionStatus(v) );

        builder
            .Property(e => e.AgeClassification)
            .HasConversion(
                v => v.ToString(),
                v => new AgeClassification(v));

        builder
            .Property(e => e.Gender)
            .HasConversion(
                v => v.ToString(),
                v => new Gender(v));

        builder
            .Property(e => e.MaritalStatus)
            .HasConversion(
                v => v.ToString(),
                v => new MaritalStatus(v));

        builder
            .Property(e => e.CommunicationPreference)
            .HasConversion(
                v => v.ToString(),
                v => new CommunicationType(v));

        // https://stackoverflow.com/questions/49176801/indexes-and-owned-types
        builder.OwnsOne(x => x.FullName, xx =>
        {
            xx.HasIndex(o => o.FirstName);
            xx.HasIndex(o => o.LastName);
        });

        // Indexes
        builder.HasIndex(o => o.ConnectionStatus);
        builder.HasIndex(x => x.RecordStatus);  // Status filtering
            
        // Configure the relationship with Church
        // If Church is deleted - all groups will be deleted
        builder
            .HasOne(p => p.Church)
            .WithMany()
            .HasForeignKey(p => p.ChurchId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
{
    public void Configure(EntityTypeBuilder<PhoneNumber> builder)
    {
        // Configure the relationship with person
        // If person is deleted - all  will be deleted
        builder
            .HasOne<Person>()
            .WithMany(p => p.PhoneNumbers)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class FollowUpConfiguration : IEntityTypeConfiguration<FollowUp>
{
    public void Configure(EntityTypeBuilder<FollowUp> builder)
    {
        // Configure the relationship with person being followed up
        // If person is deleted - all follow-ups will be deleted
        builder
            .HasOne<Person>(f => f.Person)
            .WithMany()  // No navigation property on Person side
            .HasForeignKey(f => f.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configure the relationship with person assigned to do the follow-up
        builder
            .HasOne<Person>(f => f.AssignedPerson)
            .WithMany()  // No navigation property on Person side
            .HasForeignKey(f => f.AssignedPersonId)
            .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict to avoid cascade delete conflicts
    }
}