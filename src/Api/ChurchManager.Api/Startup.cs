using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Application;
using ChurchManager.Api.Extensions;
using ChurchManager.Core;
using CodeBoss.AspNetCore;
using CodeBoss.CQRS.Commands;
using CodeBoss.CQRS.Queries;
using DbMigrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using People.Application;

namespace ChurchManager.Api
{
    public class Startup
    {
        private const string Prefix = "api";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.UseGeneralRoutePrefix(Prefix);
            });

            // TODO: Add specific environments
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins("http://localhost:4200", // Angular App
                                           "http://churchmanager.codeboss.tech") // Production
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });


            // https://developerhandbook.com/aws/how-to-use-aws-cognito-with-net-core/
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChurchManager.Api", Version = "v1" });
            });

            services.AddPeopleDomain(Configuration);
            services.AddGroupsDomain(Configuration);
            services.AddChurchManagerDbContext(Configuration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = SigningKey(
                            "p4f23ftCgFmuV23OlzxYeGygN7sXRcYrEV7puWRZGXMavGdXPPG-WrzdKxEE7i3czCIniFAENOTtIIxZtUEHVRNCaknQO4V8X5Gou43P5LpExlRMIHEXDC9-Ep3D4p73jgwu1n4Rx3ynXwz07vThoe2TUtoQVGQIa_nDZWJZm041XSpQTOz438oZ5_lDKv7i70XKGvdZMRiV0-hRCbX2Jqtk6fBizw-ZoOyZ48GbM7TZ_HyfgWHDrEQ84UYnrH_K7Es8ufuqJg96RXVRjybaQGv4ZzLbftY6uNAKMFJSsvgIzyOuAYjPoxYMRqdK1CBXQnFfkprOsgQC0qFHSEWLSw",
                            "AQAB"
                            ),

                        ValidIssuer = "https://cognito-idp.us-east-1.amazonaws.com/us-east-1_i6pWJxu8q",
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateLifetime = false,
                        ValidateAudience = false,   // Not provided by cognito,
                        RoleClaimType = "cognito:groups",
                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });

            services.AddAspNetCurrentUser<ICognitoCurrentUser, CognitoCurrentUser>();
            services.AddQueryHandlers()
                        .AddInMemoryCommandDispatcher()
                        .AddInMemoryQueryDispatcher();
        }

        public RsaSecurityKey SigningKey(string Key, string Expo)
        {
            RSA rsa = RSA.Create();

            rsa.ImportParameters(new RSAParameters
            {
                Modulus = Base64UrlEncoder.DecodeBytes(Key),
                Exponent = Base64UrlEncoder.DecodeBytes(Expo)
            });

            return new RsaSecurityKey(rsa);
        }

        TokenValidationParameters test = new()
        {
            IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
            {
                // get JsonWebKeySet from AWS
                var json = new WebClient().DownloadString("https://cognito-idp.us-east-1.amazonaws.com/us-east-1_i6pWJxu8q/.well-known/jwks.json");
                // serialize the result
                var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                // cast the result to be the type expected by IssuerSigningKeyResolver
                return (IEnumerable<SecurityKey>)keys;
            },

            ValidIssuer = "https://cognito-idp.us-east-1.amazonaws.com/us-east-1_i6pWJxu8q",
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            // Do not validate Audience on the "access" token since Cognito does not supply it but it is on the "id"
            ValidateAudience = false
        };



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChurchManager.Api v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
