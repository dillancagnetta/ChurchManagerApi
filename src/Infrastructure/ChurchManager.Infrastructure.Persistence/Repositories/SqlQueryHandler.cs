#region

using System.Data;
using System.Reflection;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class SqlQueryHandler : ISqlQueryHandler
{
    private readonly string _connectionString;
    private readonly ILogger<SqlQueryHandler> _logger;
    private const int DefaultCommandTimeout = 30;

    public SqlQueryHandler(ChurchManagerDbContext dbContext, ILogger<SqlQueryHandler> logger)
    {
        _connectionString = dbContext.Database.GetConnectionString();
        _logger = logger;
    }

    public async Task<List<T>> QueryAsync<T>(string sql, object[] parameters = null, int? timeoutSeconds = 30,
        CancellationToken ct = default) where T : class, new()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await using var command = connection.CreateCommand();

        try
        {
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = timeoutSeconds ?? DefaultCommandTimeout;

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = $"@p{i}";
                    parameter.Value = parameters[i] ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }
            }
            
            var properties = typeof(T).GetProperties()
                .Where(p => p.CanWrite)
                .ToList();
            var list = new List<T>(properties.Count);
            
            await connection.OpenAsync(ct);
            await using var result = await command.ExecuteReaderAsync(ct);

            // Create a mapping of column names to property infos
            var columnMap = new Dictionary<int, PropertyInfo>();
            for (int i = 0; i < result.FieldCount; i++)
            {
                var columnName = result.GetName(i);
                var property = properties.FirstOrDefault(p =>
                    p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
                if (property != null)
                {
                    columnMap[i] = property;
                }
            }

            while (await result.ReadAsync(ct))
            {
                var item = new T();
                foreach (var mapping in columnMap)
                {
                    if (!result.IsDBNull(mapping.Key))
                    {
                        try
                        {
                            var value = result.GetValue(mapping.Key);
                            var property = mapping.Value;
                            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                            if (value != DBNull.Value)
                            {
                                // Handle special type conversions
                                object convertedValue = targetType switch
                                {
                                    var t when t == typeof(bool) => Convert.ToBoolean(value),
                                    var t when t == typeof(int) => int.Parse(value.ToString()),
                                    var t when t == typeof(Guid) => Guid.Parse(value.ToString()),
                                    var t when t.IsEnum => Enum.Parse(t, value.ToString()),
                                    _ => Convert.ChangeType(value, targetType)
                                };

                                property.SetValue(item, convertedValue);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex,
                                "Error mapping column {Column} to property {Property}",
                                mapping.Key, mapping.Value.Name);
                        }
                    }
                }

                list.Add(item);
            }

            return list;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing SQL query: {Sql}", sql);
            throw;
        }
        finally
        {
            if (connection.State == ConnectionState.Open) await connection.CloseAsync();
        }
    }

    public Task<List<T>> QueryAsync<T>(string sql, object[] parameters = null, CancellationToken ct = default) where T : class, new()
    {
        return QueryAsync<T>(sql, parameters, DefaultCommandTimeout, ct);
    }
}