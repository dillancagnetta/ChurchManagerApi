using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Banking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class BenefactorConfiguration: IEntityTypeConfiguration<Benefactor>
{
    public void Configure(EntityTypeBuilder<Benefactor> builder)
    {
        builder.ToTable(nameof(Benefactor), "Finances");
        
        builder.HasIndex(x => x.Type);

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
        builder.ToTable(nameof(Giving), "Finances");

        builder.HasIndex(x => x.GivingType);
        builder.HasIndex(x => x.PaymentMethod);
        
        builder
            .Property(e => e.PaymentMethod)
            .HasEnumerationConversion<PaymentMethod>();
        
        // ensure that each use of the Money type in different entities has a unique configuration or naming to avoid conflicts.
        builder
            .Property(e => e.GivingType)
            .HasEnumerationConversion<GivingType>();
        
        builder.OwnsOne(t => t.GivingAmount);
        
        //  delete if Benefactor is deleted
        builder
            .HasOne(p => p.Benefactor)
            .WithMany()
            .HasForeignKey(p => p.BenefactorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Delete if Fund is deleted
        builder
            .HasOne(p => p.Fund)
            .WithMany()
            .HasForeignKey(p => p.FundId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // set null if import is deleted
        builder
            .HasOne(p => p.Import)
            .WithMany(i => i.Givings) // ✅ explicitly map the inverse
            .HasForeignKey(p => p.BankStatementImportId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    public class FundConfiguration : IEntityTypeConfiguration<Fund>
    {
        public void Configure(EntityTypeBuilder<Fund> builder)
        {
            builder.ToTable(nameof(Fund), "Finances");
            
            builder.HasIndex(x => x.FundType);
            
            builder
                .Property(e => e.FundType)
                .HasEnumerationConversion<FundType>();
        }
    }
}