using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances.Banking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class BankStatementImportConfiguration : IEntityTypeConfiguration<BankStatementImport>
{
    public void Configure(EntityTypeBuilder<BankStatementImport> builder)
    {
        builder.ToTable(nameof(BankStatementImport), "Finances");

        builder.HasIndex(x => x.BankAccount);
        
        builder
            .Property(e => e.Currency)
            .HasEnumerationConversion<Currency>();
        
    }
}

public class ImportedTransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable(nameof(Transaction), "Finances");
            
        builder.HasIndex(x => x.OriginalReference);
        builder.HasIndex(x => x.TransactionType);
        
        builder.OwnsOne(t => t.TransactionAmount);
        
        // Delete if import deleted
        builder
            .HasOne(p => p.Import)
            .WithMany()
            .HasForeignKey(p => p.ImportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}