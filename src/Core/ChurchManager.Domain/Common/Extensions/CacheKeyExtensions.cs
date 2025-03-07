using System.Text;
using CodeBoss.Extensions;
using Convey.CQRS.Queries;

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

    public static string ToCacheKey(this IPagedQuery paging) =>
        $"{paging.Page}_{paging.Results}_{paging.OrderBy}_{paging.SortOrder}";
    
    public static string ToCacheKey(this DateTime dt) => dt.ToString("yyyy-MM-dd");
    public static string ToCacheKey(this DateTime? dt) => dt.HasValue ? ToCacheKey(dt.Value) : string.Empty;
    
    public static string GenerateCacheKey(params object[] parameters)
    {
        if (parameters == null || parameters.Length == 0) return string.Empty;

        var sb = new StringBuilder();

        foreach (var param in parameters)
        {
            if (param == null) continue;
            
            // Handle nullable DateTime explicitly before the switch
            if (param is DateTime?)
            {
                var nullableDt = (DateTime?)param;
                if (nullableDt.HasValue)
                {
                    if (sb.Length > 0) sb.Append("_");
                    sb.Append(nullableDt.Value.ToCacheKey());
                }
                continue;
            }


            string keyPart = param switch
            {
                IEnumerable<int> enumerable => enumerable.ToCacheKey(),
                IEnumerable<string> enumerable => enumerable.ToCacheKey(),
                Dictionary<string, int> dict => dict.ToCacheKey(),
                IPagedQuery paging => paging.ToCacheKey(),
                _ => param.ToString()
            };

            if (!string.IsNullOrEmpty(keyPart))
            {
                if (sb.Length > 0) sb.Append("_");
                sb.Append(keyPart);
            }
        }

        return sb.ToString();
    }

}