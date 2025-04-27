using Ardalis.Specification;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Types;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class CachedDbRepository<T> : IReadDbRepository<T> where T : class, IAggregateRoot<int>
{
    private readonly IGenericDbRepository<T> _sourceRepository;
    private readonly IQueryCache _cache;
    private readonly ILogger<CachedDbRepository<T>> _logger;
    
    #region Properties

    public DbContext DbContext { get; }

    #endregion
    
    public virtual IQueryable<T> Queryable(params string[] includes) => _sourceRepository.Queryable(includes);


    public CachedDbRepository(
        IGenericDbRepository<T> dbRepository, IQueryCache cache,
        ILogger<CachedDbRepository<T>> logger)
    {
        _sourceRepository = dbRepository;
        _cache = cache;
        _logger = logger;
        
        DbContext = dbRepository.DbContext;
    }

    public Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = new CancellationToken()) where TId : notnull
    {
        return _sourceRepository.GetByIdAsync(id, cancellationToken);
    }

    [Obsolete("Use FirstOrDefaultAsync<T> or SingleOrDefaultAsync<T> instead. The SingleOrDefaultAsync<T> can be applied only to SingleResultSpecification<T> specifications.")]
    public Task<T> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = new CancellationToken())
    {
        if(specification.CacheEnabled)
        {
            string key = $"{specification.CacheKey}-GetBySpecAsync";
            _logger.LogInformation("Checking cache for " + key);
            return _cache.GetOrSetAsync(key, () =>
            {
                _logger.LogWarning("Fetching source data for " + key);
                return _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
            }, ct:cancellationToken);
        }
        return _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
    }

    [Obsolete("Use FirstOrDefaultAsync<T> or SingleOrDefaultAsync<T> instead. The SingleOrDefaultAsync<T> can be applied only to SingleResultSpecification<T> specifications.")]
    public Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if(specification.CacheEnabled)
        {
            string key = $"{specification.CacheKey}-GetBySpecAsync";
            _logger.LogInformation("Checking cache for " + key);
            return _cache.GetOrSetAsync(key, () =>
            {
                _logger.LogWarning("Fetching source data for " + key);
                return _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
            }, ct:cancellationToken);
        }
        return _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
    }

    public Task<T> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = new CancellationToken())
    {
        if (specification.CacheEnabled)
        {
            string key = $"{specification.CacheKey}-ListAsync";
            _logger.LogInformation("Checking cache for " + key);
            return _cache.GetOrSetAsync(key, () =>
            {
                _logger.LogWarning("Fetching source data for " + key);
                return _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
            }, ct: cancellationToken);
        }
        return _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
        
    }

    public Task<TResult> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (specification.CacheEnabled)
        {
            string key = $"{specification.CacheKey}-ListAsync";
            _logger.LogInformation("Checking cache for " + key);
            return _cache.GetOrSetAsync(key, () =>
            {
                _logger.LogWarning("Fetching source data for " + key);
                return _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
            }, ct: cancellationToken);
        }
        return _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
        
    }

    public Task<T> SingleOrDefaultAsync(ISingleResultSpecification<T> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (specification.CacheEnabled)
        {
            string key = $"{specification.CacheKey}-ListAsync";
            _logger.LogInformation("Checking cache for " + key);
            return _cache.GetOrSetAsync(key, () =>
            {
                _logger.LogWarning("Fetching source data for " + key);
                return _sourceRepository.SingleOrDefaultAsync(specification, cancellationToken);
            }, ct: cancellationToken);
        }
        return _sourceRepository.SingleOrDefaultAsync(specification, cancellationToken);
        
    }

    public Task<TResult> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (specification.CacheEnabled)
        {
            string key = $"{specification.CacheKey}-ListAsync";
            _logger.LogInformation("Checking cache for " + key);
            return _cache.GetOrSetAsync(key, () =>
            {
                _logger.LogWarning("Fetching source data for " + key);
                return _sourceRepository.SingleOrDefaultAsync(specification, cancellationToken);
            }, ct: cancellationToken);
        }
        return _sourceRepository.SingleOrDefaultAsync(specification, cancellationToken);
    }

    public Task<List<T>> ListAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        string key = $"{nameof(T)}-ListAsync";
        return _cache.GetOrSetAsync(key, () => _sourceRepository.ListAsync(cancellationToken), ct: cancellationToken);
    }

    public Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = new CancellationToken())
    {
        if (specification.CacheEnabled)
        {
            string key = $"{specification.CacheKey}-ListAsync";
            _logger.LogInformation("Checking cache for " + key);
            return _cache.GetOrSetAsync(key, () =>
            {
                _logger.LogWarning("Fetching source data for " + key);
                return _sourceRepository.ListAsync(specification, cancellationToken);
            }, ct: cancellationToken);
        }
        return _sourceRepository.ListAsync(specification, cancellationToken);
        
    }

    public Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = new CancellationToken())
    {
        if (specification.CacheEnabled)
        {
            string key = $"{specification.CacheKey}-ListAsync";
            _logger.LogInformation("Checking cache for " + key);
            return _cache.GetOrSetAsync(key, () =>
            {
                _logger.LogWarning("Fetching source data for " + key);
                return _sourceRepository.ListAsync(specification, cancellationToken);
            }, ct: cancellationToken);
        }
        return _sourceRepository.ListAsync(specification, cancellationToken);
    }

    public Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = new CancellationToken())
    {
        return _sourceRepository.CountAsync(specification, cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _sourceRepository.CountAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = new CancellationToken())
    {
        return _sourceRepository.AnyAsync(specification, cancellationToken);
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _sourceRepository.AnyAsync(cancellationToken);
    }

    public IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification)
    {
        return _sourceRepository.AsAsyncEnumerable(specification);
    }

    public Task<PagedResult<T>> BrowseAsync(IPagedQuery query, ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        if (specification.CacheEnabled)
        {
            string key = $"{specification.CacheKey}-ListAsync";
            _logger.LogInformation("Checking cache for " + key);
            return _cache.GetOrSetAsync(key, () =>
            {
                _logger.LogWarning("Fetching source data for " + key);
                return _sourceRepository.BrowseAsync(query, specification, cancellationToken);
            }, ct: cancellationToken);
        }
        return _sourceRepository.BrowseAsync(query, specification, cancellationToken);
    }

    public Task<PagedResult<TResult>> BrowseAsync<TResult>(IPagedQuery query, ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        if (specification.CacheEnabled)
        {
            string key = $"{specification.CacheKey}-ListAsync";
            _logger.LogInformation("Checking cache for " + key);
            return _cache.GetOrSetAsync(key, () =>
            {
                _logger.LogWarning("Fetching source data for " + key);
                return _sourceRepository.BrowseAsync(query, specification, cancellationToken);
            }, ct: cancellationToken);
        }
        return _sourceRepository.BrowseAsync(query, specification, cancellationToken);
    }
}