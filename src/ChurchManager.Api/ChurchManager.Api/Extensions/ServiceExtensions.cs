﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json;
using ChurchManager.Api.Models;
using Convey;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
                    },
                });
            });
        }

        public static void AddControllersExtension(this IServiceCollection services, string prefix)
        {
            services.AddControllers(options =>
                {
                    options.UseGeneralRoutePrefix(prefix);
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
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
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins(
                            "http://localhost:4200", // Angular App
                            "http://churchmanager.codeboss.tech" // Production
                            ) 
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

        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetOptions<JwtSettings>(nameof(JwtSettings));

            // https://developerhandbook.com/aws/how-to-use-aws-cognito-with-net-core/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = SigningKey(
                            jwtSettings.Key,
                            "AQAB"
                        ),

                        ValidIssuer = jwtSettings.Issuer,
                        //ValidAudience = jwtSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateLifetime = false,
                        ValidateAudience = false,   // Not provided by cognito,
                        RoleClaimType = "cognito:groups",
                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });

            RsaSecurityKey SigningKey(string Key, string Expo)
            {
                RSA rsa = RSA.Create();

                rsa.ImportParameters(new RSAParameters
                {
                    Modulus = Base64UrlEncoder.DecodeBytes(Key),
                    Exponent = Base64UrlEncoder.DecodeBytes(Expo)
                });

                return new RsaSecurityKey(rsa);
            }
        }
    }
}