using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.MultiTenancy;
using ChurchManager.TenantManager.Data;
using ChurchManager.TenantManager.Services;
using CodeBoss.AspNetCore.DependencyInjection;
using CodeBoss.MultiTenant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared._DependencyInjection
{
    public class MultiTenancyDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddCodeBossMultiTenancy<TenantConfiguration>(configuration, opt =>
            {
                opt.TenantsProvider = typeof(MasterDbTenantProvider);
            });
            
            // Injected into db context to provide UserLoginId info
            services.AddScoped<ITenantCurrentUser, SimpleCurrentUser>();
            services.AddScoped<ITenantUrlResolver, TenantUrlResolver>();
        }
    }
}
