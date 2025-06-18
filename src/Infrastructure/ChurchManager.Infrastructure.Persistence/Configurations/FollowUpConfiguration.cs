using ChurchManager.Domain.Features.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations;

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