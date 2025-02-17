﻿using System.Reflection;
using ChurchManager.Features.Common.Behaviours;
using ChurchManager.SharedKernel.Common;
using CodeBoss.AspNetCore;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Features.Common
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            //services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(OpenTelemetryBehavior<,>));

            #region Application Services


            #endregion

            services.AddAspNetCurrentUser<ICognitoCurrentUser, CognitoCurrentUser>(); }
    }
}