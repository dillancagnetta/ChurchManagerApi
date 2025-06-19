using ChurchManager.Domain.Features.Finances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class BenefactorConfiguration: IEntityTypeConfiguration<Benefactor>
{
    public void Configure(EntityTypeBuilder<Benefactor> builder)
    {
        builder
            .Property(e => e.Type)
            .HasEnumerationConversion<BenefactorType>();
        
        // Configure the relationship with Church
        // If Church is deleted - all benefactors will set to null to preserve
        builder
            .HasOne(p => p.Church)
            .WithMany()
            .HasForeignKey(p => p.ChurchId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Configure the relationship with Person
        // If Person is deleted - all benefactors will set to null to preserve
        builder
            .HasOne(p => p.Person)
            .WithMany()
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Configure the relationship with Group
        // If Group is deleted - all benefactors will set to null to preserve
        builder
            .HasOne(p => p.Group)
            .WithMany()
            .HasForeignKey(p => p.GroupId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Configure the relationship with Family
        // If Family is deleted - all benefactors will set to null to preserve
        builder
            .HasOne(p => p.Family)
            .WithMany()
            .HasForeignKey(p => p.FamilyId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}


public class GivingConfiguration: IEntityTypeConfiguration<Giving>
{
    public void Configure(EntityTypeBuilder<Giving> builder)
    {
        builder
            .Property(e => e.PaymentMethod)
            .HasEnumerationConversion<PaymentMethod>();
        
        builder
            .Property(e => e.GivingType)
            .HasEnumerationConversion<GivingType>();
        
        builder.OwnsOne(x => x.Amount);
        
        // Dont delete if Benefactor is deleted
        builder
            .HasOne(p => p.Benefactor)
            .WithMany()
            .HasForeignKey(p => p.BenefactorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Delete if Fund is deleted
        builder
            .HasOne(p => p.Fund)
            .WithMany()
            .HasForeignKey(p => p.BenefactorId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public class FundConfiguration : IEntityTypeConfiguration<Fund>
    {
        public void Configure(EntityTypeBuilder<Fund> builder)
        {
            builder
                .Property(e => e.Type)
                .HasEnumerationConversion<FundType>();

            // SetNull if Partnership is deleted
            builder
                .HasOne(p => p.Partnership)
                .WithMany()
                .HasForeignKey(p => p.PartnershipId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}