using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Extensions;
using Codeboss.Types;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class CacheGenericDbRepository<T> : GenericRepositoryBase<T> where T : class, IAggregateRoot<int>
{
    #region Fields

    internal DbSet<T> ObjectSet;
    private readonly ISpecificationEvaluator _specificationEvaluator;
    private readonly IQueryCache _cache;
    
    #endregion

    #region Properties

    public DbContext DbContext { get; }

    #endregion

    public CacheGenericDbRepository(DbContext dbContext, IQueryCache cache) : this(dbContext, SpecificationEvaluator.Default)
    {
        _cache = cache;
    }

    public CacheGenericDbRepository(DbContext dbContext, ISpecificationEvaluator specificationEvaluator) : base(dbContext, specificationEvaluator)
    {
        DbContext = dbContext;
        ObjectSet = DbContext.Set<T>();
        _specificationEvaluator = specificationEvaluator;
    }

    public override Task<T?> GetByIdAsync<TId>(TId id, CancellationToken ct = default)
    {
        var cacheKey = CacheKeyHelper.CacheKey<T>(id.ToString());
            
        return _cache.GetOrSetAsync(cacheKey, async () => await base.GetByIdAsync(id, ct), ct: ct);
    }

    public override Task<PagedResult<T>> BrowseAsync(IPagedQuery query, ISpecification<T> specification,
        CancellationToken ct = default)
    {
        var cacheKey = CacheKeyHelper.CacheKey("BrowseAsync" ,query, specification);
        
        return _cache.GetOrSetAsync(cacheKey, async () => await base.BrowseAsync(query, specification, ct), ct: ct);
    }

    public override Task<PagedResult<TResult>> BrowseAsync<TResult>(IPagedQuery query,
        ISpecification<T, TResult> specification, CancellationToken ct = default)
    {
        var cacheKey = CacheKeyHelper.CacheKey("BrowseAsync" ,query, specification);
        
        return _cache.GetOrSetAsync(cacheKey, async () => await base.BrowseAsync(query, specification, ct), ct: ct);
    }
}