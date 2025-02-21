using CodeBoss.Extensions;

namespace ChurchManager.Domain.Common.Extensions;

public static class CacheKeyExtensions
{
    public static string ToCacheKey<T>(this IEnumerable<T> values)
    {
        if(values.IsNullOrEmpty()) return string.Empty;
        
        return string.Join("_", values);
    }

    public static string ToCacheKey(this Dictionary<string, int> data)
    {
        if(data.IsNullOrEmpty()) return string.Empty;
        
        return string.Join("_", data.Select(kv => $"{kv.Key}-{kv.Value}"));
    }
}