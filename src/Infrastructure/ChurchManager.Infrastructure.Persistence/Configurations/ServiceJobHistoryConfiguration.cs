#region

using CodeBoss.Jobs.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class ServiceJobHistoryConfiguration: IEntityTypeConfiguration<ServiceJobHistory>
{
    public void Configure(EntityTypeBuilder<ServiceJobHistory> builder)
    {
        builder
            .HasOne( t => t.ServiceJob )
            .WithMany( t => t.ServiceJobHistory )
            .HasForeignKey( t => t.ServiceJobId );
    }
}