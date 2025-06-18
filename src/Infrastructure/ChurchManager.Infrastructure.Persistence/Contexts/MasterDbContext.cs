using ChurchManager.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts;

public class MasterDbContext : DbContext
{
    public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options) { }
    
    public DbSet<TenantConfiguration> Tenants { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TenantConfiguration>()
            .HasKey(t => t.Id)
            ;
        
        modelBuilder.Entity<TenantConfiguration>()
            .HasIndex(t => t.Name)
            .IsUnique()
            ;
    }
}