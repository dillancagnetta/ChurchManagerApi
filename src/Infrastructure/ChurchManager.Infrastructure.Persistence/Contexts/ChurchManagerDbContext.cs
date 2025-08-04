#region

using System.Diagnostics.CodeAnalysis;
using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Abstractions.AppContext;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Shared;
using CodeBoss.MultiTenant;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext : DbContext, IChurchManagerDbContext
    {
        private ITenant _tenant;
        private readonly IDomainEventPublisher? _events;
        private readonly IAppContextAccessor? _context;
        private readonly ITenantsProvider<TenantConfiguration> _tenantsProvider;
        private readonly ITenantCurrentUser? _currentUser;

        public ChurchManagerDbContext(
            DbContextOptions<ChurchManagerDbContext> options,
            ITenantsProvider<TenantConfiguration> tenantsProvider,
            IAppContextAccessor? appContext = null,
            IDomainEventPublisher? events = null,
            ITenantCurrentUser? currentUser = null) : base(options)
        {
            _events = events;
            _context = appContext;
            _tenantsProvider = tenantsProvider;
            _currentUser = currentUser;

            if (_tenantsProvider is null) throw new ArgumentNullException(nameof(_tenantsProvider), "Valid tenantsProvider not found or configured");

            if (_tenantsProvider.Enabled)
            {
                ConfigureMultiTenants();
            }
            // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Only configure if its not already, which means we are in multi tenant mode
            if (optionsBuilder.IsConfigured == false && _tenantsProvider is not null && _tenant is not null)
            {
                optionsBuilder.UseNpgsql(_tenant.ConnectionString,
                    x => x.MigrationsAssembly("ChurchManager.Infrastructure.Persistence"));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChurchManagerDbContext).Assembly);
            //modelBuilder.HasPostgresExtension("pg_trgm");
        }   

        public override int SaveChanges()
        {
            _PreSaveChanges();
            var result = base.SaveChanges();
            _PostSaveChanges().GetAwaiter().GetResult();
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            _PreSaveChanges(ct);
            var result = await base.SaveChangesAsync(ct);
            await _PostSaveChanges(ct);
            return result;
        }

        private void ConfigureMultiTenants()
        {
            // Current user will have the tenant claim
            if(_currentUser is not null)
            {
                _tenant = _tenantsProvider.Get(_currentUser.Tenant);
            }

            // CurrentTenant is set in `TenantIdentifierMiddleware`
            // Fallback: first tenant in the list
            _tenant 
                ??= _context?.AppContext?.CurrentTenant
                    ?? _tenantsProvider.Tenants().FirstOrDefault()
                    ?? throw new ArgumentNullException(nameof(_tenant), "Valid tenant not found or configured");
        }

        private void _PreSaveChanges(CancellationToken ct = default)
        {
            _AddAuditInfo();
        }

        private async Task _PostSaveChanges(CancellationToken ct = default)
        {
            await _DispatchDomainEvents(ct);
        }

        private void _AddAuditInfo()
        {
            if(_currentUser is null)
                return;

            foreach(var entry in ChangeTracker.Entries<AuditableEntity<int>>())
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _currentUser.Id;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedDate = DateTime.UtcNow;
                        entry.Entity.ModifiedBy = _currentUser.Id;
                        break;
                }
            }
        }

        private async Task _DispatchDomainEvents(CancellationToken ct = default)
        {
            // ignore events if no dispatcher provided
            if(_events is null) return;

            var domainEventEntities = ChangeTracker.Entries<Entity<int>>()
                .Select(po => po.Entity)
                .Where(po => po.DomainEvents.Any())
                .ToArray();

            foreach(var entity in domainEventEntities)
            {
                foreach (var @event in entity.DomainEvents)
                {
                    await _events.PublishAsync(@event, ct);
                }
                entity.ClearDomainEvents();
            }
        }
    }
}
