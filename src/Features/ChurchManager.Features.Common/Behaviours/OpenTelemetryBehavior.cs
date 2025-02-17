﻿using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Shared.OpenTelemetry;
using MediatR;
using Microsoft.Extensions.Options;

namespace ChurchManager.Features.Common.Behaviours
{
    // https://garywoodfine.com/how-to-use-mediatr-pipeline-behaviours/
    public class OpenTelemetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly OpenTelemetryOptions _options;

        public OpenTelemetryBehavior(IOptions<OpenTelemetryOptions> options)
        {
            _options = options.Value;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            
            if (_options.Enabled)
            {
                // Start Telemetry Activity
                using var activity = DomainConstants.Telemetry.ActivitySource.StartActivity(typeof(TRequest).Name);

                // Process Handler
                var response = await next();

                // Stop Telemetry Activity
                activity?.Stop();

                return response;
            }
            // Not enabled
            return await next();
        }
    }
}