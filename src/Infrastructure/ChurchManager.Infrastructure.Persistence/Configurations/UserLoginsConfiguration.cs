#region

using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Configurations;

public class UserLoginsConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder
            .Property(e => e.RecordStatus)
            .HasRecordStatus();
        
        // Indexes
        builder.HasIndex(x => x.Username).IsUnique();  // Username lookups
        builder.HasIndex(x => new { x.Tenant, x.Username });  // Tenant-specific username lookups
        builder.HasIndex(x => x.PersonId);  // Person lookups
        builder.HasIndex(x => new { x.Tenant, x.RecordStatus });  // Filtered queries by tenant and status
    }
}   

public class UserLoginRoleConfiguration : IEntityTypeConfiguration<UserLoginRole>
{
    public void Configure(EntityTypeBuilder<UserLoginRole> builder)
    {
        // Indexes
        builder.HasIndex(x => x.Name);  // Role name lookups
        builder.HasIndex(x => new { x.Name, x.RecordStatus });  // Active role lookups by name
        builder.HasIndex(x => new { x.IsSystem, x.RecordStatus });  // System role queries
        builder.HasIndex(x => x.RecordStatus);  // Status filtering
    }
}

public class EntityPermissionConfiguration : IEntityTypeConfiguration<EntityPermission>
{
    public void Configure(EntityTypeBuilder<EntityPermission> builder)
    {
        // Indexes
        builder.HasIndex(x => x.EntityType);  // Entity type lookups
        builder.HasIndex(x => new { x.EntityType, x.RecordStatus });  // Active permissions by entity type
        builder.HasIndex(x => new { x.ScopeType, x.ScopeId });  // Dynamic scope lookups
        builder.HasIndex(x => new { x.IsSystem, x.RecordStatus });  // System permission queries
        
        // GIN index if you need to query the EntityIds JSON array
        builder.HasIndex(x => x.EntityIds).HasMethod("gin");  // JSON array queries
    }
}

public class UserRoleAssignmentConfiguration : IEntityTypeConfiguration<UserRoleAssignment>
{
    public void Configure(EntityTypeBuilder<UserRoleAssignment> builder)
    {
        // Unique constraint/index
        builder.HasIndex(x => new { x.UserLoginId, x.UserLoginRoleId })
            .IsUnique();
            
        // Additional indexes
        builder.HasIndex(x => x.UserLoginId);  // User's roles lookups
        builder.HasIndex(x => x.UserLoginRoleId);  // Role's users lookups
        builder.HasIndex(x => new { x.UserLoginId, x.RecordStatus });  // Active role assignments for user
    }
}

public class RolePermissionAssignmentConfiguration : IEntityTypeConfiguration<RolePermissionAssignment>
{
    public void Configure(EntityTypeBuilder<RolePermissionAssignment> builder)
    {
        // Unique constraint/index
        builder.HasIndex(x => new { x.RoleId, x.EntityPermissionId })
            .IsUnique();
            
        // Additional indexes
        builder.HasIndex(x => x.RoleId);  // Role's permissions lookups
        builder.HasIndex(x => x.EntityPermissionId);  // Permission's roles lookups
        builder.HasIndex(x => new { x.RoleId, x.RecordStatus });  // Active permission assignments for role
    }
}