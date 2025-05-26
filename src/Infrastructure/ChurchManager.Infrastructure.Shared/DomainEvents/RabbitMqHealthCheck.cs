using ChurchManager.Infrastructure.Abstractions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Wolverine.Runtime;
using Wolverine.Transports;

namespace ChurchManager.Infrastructure.Shared.DomainEvents;

public class RabbitMqHealthCheck(IWolverineRuntime wolverineRuntime, IOptions<AppConfig> options) : IHealthCheck
{
    
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var data = new Dictionary<string, object>
        {
            ["RabbitMqEnabled"] = options.Value.RabbitMqEnabled
        };
        
        if (!options.Value.RabbitMqEnabled)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Wolverine RabbitMQ integration is disabled", data));
        }
        
        try
        {
            // Get information about Wolverine's endpoints
            var endpoints = wolverineRuntime.Endpoints;
            // TODO: fix
            var rabbitMqEndpoints = endpoints.ActiveListeners();

            if (rabbitMqEndpoints.All(x => x.Status == ListeningStatus.Accepting && x.Endpoint.EndpointName.Contains("rabbitmq")))    
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("No RabbitMQ endpoints configured", data:data));
            }

            return Task.FromResult(HealthCheckResult.Healthy("Wolverine RabbitMQ integration is healthy", data:data));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("Wolverine RabbitMQ health check failed", ex, data:data));
        }
    }
}