using Ardalis.Specification;
using Convey.CQRS.Queries;

namespace ChurchManager.Infrastructure.Persistence.Extensions;

public static class CacheKeyHelper
{
    public static string CacheKey<T>(Guid id) => $"{typeof(T).Name.ToLower()}_{id}";
    public static string CacheKey<T>(string key) => $"{typeof(T).Name.ToLower()}_{key.ToLower()}";
    public static string CacheKey(string key) => $"{key.ToLower()}";
    public static string CacheKey<T>(IPagedQuery key) => $"{typeof(T).Name.ToLower()}_{key.Page}_{key.Results}_{key.OrderBy}_{key.SortOrder}";
    public static string CacheKey<T>(string key, IPagedQuery page, ISpecification<T> spec) => $"{typeof(T).Name.ToLower()}_{key.ToLower()}_{page.Page}_{page.Results}_{page.OrderBy}_{page.SortOrder}_{spec.GetType().Name.ToLower()}";
}