#region

using ChurchManager.Domain.Features.Communications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .Property(e => e.Classification)
                .HasConversion(
                    v => v.ToString(),
                    v => new MessageClassification(v));
            
            builder
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => new MessageStatus(v));
            
            builder
                .HasOne(cg => cg.UserLogin)
                .WithMany()
                .HasForeignKey(cg => cg.UserId)
                .IsRequired();
        }
    }
}