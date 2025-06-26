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

public class ImportedTransactionConfiguration : IEntityTypeConfiguration<ImportedTransaction>
{
    public void Configure(EntityTypeBuilder<ImportedTransaction> builder)
    {
        builder.ToTable(nameof(ImportedTransaction), "Finances");
            
        builder.HasIndex(x => x.OriginalReference);
        builder.HasIndex(x => x.TransactionType);
        
        builder.OwnsOne(x => x.Amount);
        
        // Delete if import deleted
        builder
            .HasOne(p => p.Import)
            .WithMany()
            .HasForeignKey(p => p.ImportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}