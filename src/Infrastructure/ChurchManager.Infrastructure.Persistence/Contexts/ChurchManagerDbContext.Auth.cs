#region

using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Security;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<UserLoginRole> UserLoginRole { get; set; }
        public DbSet<EntityPermission> EntityPermission { get; set; }
        public DbSet<UserRoleAssignment> UserRoleAssignment { get; set; }
        public DbSet<RolePermissionAssignment> RolePermissionAssignment { get; set; }
    }
}
