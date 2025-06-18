using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ChurchManager.Api.Middlewares;


public static class HealthCheckResponseWriter
{
    public static async Task WriteResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        
        var response = new
        {
            status = healthReport.Status.ToString(),
            totalDuration = healthReport.TotalDuration,
            results = healthReport.Entries.Select(x => new
            {
                key = x.Key,
                status = x.Value.Status.ToString(),
                description = x.Value.Description,
                duration = x.Value.Duration,
                exception = x.Value.Exception?.Message,
                data = x.Value.Data,
                tags = x.Value.Tags?.ToArray()
            })
        };
        
        var jsonString = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(jsonString);
    }
}