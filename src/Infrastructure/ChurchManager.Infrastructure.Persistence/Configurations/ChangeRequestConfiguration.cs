using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.ChangeRequests;
using ChurchManager.Domain.Features.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ChangeRequestConfiguration: IEntityTypeConfiguration<ChangeRequest>
{
    public void Configure(EntityTypeBuilder<ChangeRequest> builder)
    {
        builder
            .Property(e => e.Status)
            .HasEnumerationConversion<ApprovalStatus>();
        
        builder.HasIndex(x => x.ChurchId);
        
        // Configure the relationship with person requesting change
        builder
            .HasOne<Person>(f => f.ReviewedByPerson)
            .WithMany()  // No navigation property on Person side
            .HasForeignKey(f => f.ReviewedByPersonId)
            .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict to avoid cascade delete conflicts
        
        // Configure the relationship with Church
        // If Church is deleted - all change requests will set to null to preserve
        builder
            .HasOne(p => p.Church)
            .WithMany()
            .HasForeignKey(p => p.ChurchId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class PropertyChangeRequestConfiguration: IEntityTypeConfiguration<PropertyChangeRequest>
{
    public void Configure(EntityTypeBuilder<PropertyChangeRequest> builder)
    {
        builder.HasIndex(x => x.EntityType);
        builder.HasIndex(x => x.EntityId);
        builder.HasIndex(x => x.PropertyPath);
        builder.HasIndex(x => x.IsApplied);
    }
}