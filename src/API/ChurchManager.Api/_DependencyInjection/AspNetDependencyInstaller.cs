using System.Threading.RateLimiting;
using ChurchManager.Api.Extensions;
using ChurchManager.Api.Middlewares;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.AspNetCore.RateLimiting;

namespace ChurchManager.Api._DependencyInjection
{
    public class AspNetDependencyInstaller : IDependencyInstaller
    {
        private const string Prefix = "api";

        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddCorsExtension();
            services.AddControllersExtension(Prefix);
            services.AddSwaggerExtension();

            // API version
            services.AddApiVersioningExtension();
            // API explorer
            services.AddMvcCore(options => {
                    options.Filters.Add<OperationCancelledExceptionFilter>();
                })
                .AddApiExplorer();
            // API explorer version
            services.AddVersionedApiExplorerExtension();

            services.AddHealthChecks();
            
            // Does not register it globally but only where it is applied
            services.AddScoped<AwsIpFilterAttribute>();

            services.AddRateLimiter(options =>
            {
                // Global rate limiter - applies to all endpoints
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                    httpContext => RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1)
                        }));
                
                // Custom partition key resolver
                options.AddPolicy("per-user", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity?.Name ?? "anonymous",
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1)
                        }));
                
                /*
                    - Each request consumes one token from the bucket
                    - If the bucket is empty, the request is rejected or queued
                    
                    controller attribute: [EnableRateLimiting("strict")]
                 */
                options.AddTokenBucketLimiter("strict", limiterOptions =>
                {
                    limiterOptions.TokenLimit = 100;
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 0;
                    limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
                    limiterOptions.TokensPerPeriod = 20;
                    limiterOptions.AutoReplenishment = true;
                });

                /*
                    Divides the time window into smaller segments and tracks requests across these segments.
                    This provides smoother rate limiting compared to fixed windows.
                 */
                options.AddSlidingWindowLimiter("sliding", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 50;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.SegmentsPerWindow = 6;
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 10;
                });
                
                // Strict rate limiting for public endpoints
                options.AddPolicy("public-endpoint", httpContext =>
                    RateLimitPartition.GetSlidingWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: partition => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 10,                                    // 10 requests per minute
                            Window = TimeSpan.FromMinutes(1),
                            SegmentsPerWindow = 6,                               // 10-second segments
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 2
                        }));

                // Custom rejection response
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", 
                        cancellationToken: token);
                };
                
                // Custom rejection response
                options.RejectionStatusCode = 429;
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Try again later.", cancellationToken: token);
                };
            });
        }
    }
}
