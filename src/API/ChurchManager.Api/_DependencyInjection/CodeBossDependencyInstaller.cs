﻿using CodeBoss.AspNetCore;
using CodeBoss.AspNetCore.DependencyInjection;

namespace ChurchManager.Api._DependencyInjection
{
    public class CodeBossDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddCodeBossDateTime(configuration);
        }
    }
}
