namespace ChurchManager.Infrastructure.Abstractions.Persistence;

public interface ISqlQueryHandler
{
    Task<List<T>> QueryAsync<T>(string sql, object[] parameters = null, int? timeoutSeconds = 30, CancellationToken ct = default) where T : class, new();
    Task<List<T>> QueryAsync<T>(string sql, object[] parameters = null, CancellationToken ct = default) where T : class, new();
}