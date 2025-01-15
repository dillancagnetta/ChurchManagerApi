using ChurchManager.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts;

public partial class AuthDbContext : DbContext
{
    public DbSet<UserLogin> UserLogin { get; set; }
}