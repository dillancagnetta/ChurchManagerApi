using System.Text.Json;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Communications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class CommunicationConfiguration : IEntityTypeConfiguration<Communication>
{
    public void Configure(EntityTypeBuilder<Communication> builder)
    {
        builder
            .Property(e => e.CommunicationType)
            .HasEnumerationConversion<CommunicationType>();

        builder
            .Property(e => e.Status)
            .HasEnumerationConversion<CommunicationStatus>();
        
        builder
            .Property(e => e.RecordStatus)
            .HasRecordStatus();

        builder.OwnsOne(x => x.Review);

        /*builder.HasOne(c => c.Content)
            .WithOne(cc => cc.Communication)
            .HasForeignKey<CommunicationContent>(cc => cc.CommunicationId)
            .OnDelete(DeleteBehavior.Cascade);*/

        builder.HasMany(c => c.Recipients)
            .WithOne(r => r.Communication)
            .HasForeignKey(r => r.CommunicationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(c => c.Attachments)
            .WithOne(r => r.Communication)
            .HasForeignKey(r => r.CommunicationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Use JSON serialization for complex properties
        builder.Property(c => c.Metadata)
            .HasColumnType("jsonb") // for Postgres
            .HasJsonConversion(options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            .Metadata.SetValueComparer(new JsonValueComparer<Dictionary<string, object>>());
        
        /* Optional Properties #1# */
        builder.HasOne(r => r.SystemCommunication)
            .WithMany()
            .HasForeignKey(r => r.SystemCommunicationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.CommunicationTemplate)
            .WithMany()
            .HasForeignKey(r => r.CommunicationTemplateId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.ListGroup)
            .WithMany()
            .HasForeignKey(r => r.ListGroupId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.SenderPerson)
            .WithMany()
            .HasForeignKey(r => r.SenderPersonId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

/*
 * ------------------------------------
 */

public class CommunicationTemplateConfiguration : IEntityTypeConfiguration<CommunicationTemplate>
{
    public void Configure(EntityTypeBuilder<CommunicationTemplate> builder)
    {
        builder
            .Property(e => e.RecordStatus)
            .HasRecordStatus();

        builder
            .Property(e => e.SupportedTypes)
            .HasEnumerationListConversion<CommunicationType>()
            .HasColumnType("text")
            .Metadata.SetValueComparer(new EnumerationListValueComparer<CommunicationType>());
    }
}

/*
 * ------------------------------------
 */
 
public class CommunicationRecipientConfiguration : IEntityTypeConfiguration<CommunicationRecipient>
{
    public void Configure(EntityTypeBuilder<CommunicationRecipient> builder)
    {
        builder
            .Property(e => e.RecordStatus)
            .HasRecordStatus();

        builder
            .Property(e => e.Status)
            .HasEnumerationConversion<CommunicationRecipientStatus>();
        
        builder.HasOne(r => r.RecipientPerson)
            .WithMany()
            .HasForeignKey(r => r.PersonId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}

/*
 * ------------------------------------
 */
 
public class CommunicationAttachmentConfiguration : IEntityTypeConfiguration<CommunicationAttachment>
{
    public void Configure(EntityTypeBuilder<CommunicationAttachment> builder)
    {
        builder
            .Property(e => e.RecordStatus)
            .HasRecordStatus();

        builder
            .Property(e => e.CommunicationType)
            .HasEnumerationConversion<CommunicationType>();
    }
}