using Ardalis.Specification;
using Convey.CQRS.Queries;

namespace ChurchManager.Infrastructure.Persistence.Extensions;

public static class CacheKeyHelper
{
    public static string CacheKey<T>(Guid id) => $"{typeof(T).Name}_{id}";
    public static string CacheKey<T>(string key) => $"{typeof(T).Name}_{key}";
    public static string CacheKey(string key) => $"{key}";
    public static string CacheKey<T>(IPagedQuery key) => $"{typeof(T).Name}_{key.Page}_{key.Results}_{key.OrderBy}_{key.SortOrder}";
    public static string CacheKey<T>(string key, IPagedQuery page, ISpecification<T> spec) => $"{typeof(T).Name}_{key}_{page.Page}_{page.Results}_{page.OrderBy}_{page.SortOrder}_{spec.GetType().Name}";
}