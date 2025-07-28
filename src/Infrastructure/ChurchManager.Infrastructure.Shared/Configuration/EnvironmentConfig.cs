using ChurchManager.Infrastructure.Abstractions.Configuration;

namespace ChurchManager.Infrastructure.Shared.Configuration;

public class EnvironmentConfig : IEnvironmentConfig
{
    private const string Development = "Development";
    private const string Production = "Production";
    public string EnvironmentName { get; } = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Development;
    public string SubdomainKey { get; } = Environment.GetEnvironmentVariable("SUBDOMAIN_KEY")?.ToLowerInvariant() ?? "localhost";

    public bool IsDevelopment => EnvironmentName.Equals(Development, StringComparison.CurrentCultureIgnoreCase);
    
    public bool IsProduction => EnvironmentName.Equals(Production, StringComparison.CurrentCultureIgnoreCase);

    public string? GetValue(string key) => Environment.GetEnvironmentVariable(key.ToUpper());
}