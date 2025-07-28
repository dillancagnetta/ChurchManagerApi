namespace ChurchManager.Infrastructure.Abstractions.Configuration;

public interface IEnvironmentConfig
{
    string EnvironmentName { get; }
    string SubdomainKey { get; }
    bool IsDevelopment { get; }
    bool IsProduction{ get; }
    string? GetValue(string key);
}