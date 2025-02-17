﻿using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChurchManager.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ChurchManager.Api",
                    Contact = new OpenApiContact { Name = "Dillan Cagnetta", Email = "connect@codeboss.co.za"}
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Format: Bearer {your token here}",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    }
                });


                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void AddControllersExtension(this IServiceCollection services, string prefix)
        {
            services.AddControllers(options =>
                {
                    options.UseGeneralRoutePrefix(prefix);
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }

        //Configure CORS to allow any origin, header and method. 
        //Change the CORS policy based on your requirements.
        //More info see: https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.0

        public static void AddCorsExtension(this IServiceCollection services)
        {
            // TODO: Add specific environments
            services.AddCors(options =>
            {
                //  If you need the top level as well as subdomains, you need to add both -> e.g. .WithOrigins(new string[] { "https://*.example.com", "https://example.com" }) 
                options.AddPolicy(ApiRoutes.DefaultCorsPolicy,
                    builder => builder
                        .WithOrigins(
                            "http://localhost:4200", // Angular App
                            "http://localhost:8080", // Angular PWA test
                            "http://*.codeboss.tech", // HTTP
                            "https://*.codeboss.tech", // HTTPS
                            "http://*.codeboss.co.za", // HTTP
                            "https://*.codeboss.co.za", // HTTPS
                            "http://codeboss.tech", // TOP LEVEL DOMAIN
                            "https://codeboss.tech", // TOP LEVEL DOMAIN
                            "http://codeboss.co.za", // TOP LEVEL DOMAIN
                            "https://codeboss.co.za", // TOP LEVEL DOMAIN
                            "http://*.codeboss.tech.s3-website-us-east-1.amazonaws.com", // S3 buckets
                            "https://*.netlify.app", // NETLIFY
                            "http://dxoazadshajgs.cloudfront.net", // Test Cloud front
                            "https://dxoazadshajgs.cloudfront.net" // Test Cloud front
                            )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        public static void AddVersionedApiExplorerExtension(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });
        }
        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
        }
    }
}
