#region

using System.Text.Json;
using CodeBoss.Jobs.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class ServiceJobHistoryConfiguration: IEntityTypeConfiguration<ServiceJobHistory>
{
    public void Configure(EntityTypeBuilder<ServiceJobHistory> builder)
    {
        builder.ToTable(nameof(ServiceJobHistory), "Jobs");

        builder
            .HasOne( t => t.ServiceJob )
            .WithMany( t => t.ServiceJobHistory )
            .HasForeignKey( t => t.ServiceJobId );
    }
}

public class ServiceJobConfiguration: IEntityTypeConfiguration<ServiceJob>
{
    public void Configure(EntityTypeBuilder<ServiceJob> builder)
    {
        builder.ToTable(nameof(ServiceJob), "Jobs");

        builder.Property(c => c.JobParameters)
            .HasColumnType("jsonb") // for Postgres
            .HasJsonConversion(options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            .Metadata.SetValueComparer(new JsonValueComparer<Dictionary<string, string>>());
        
        builder.HasIndex(x => x.Name); 
        builder.HasIndex(x => x.JobKey); 
    }
}