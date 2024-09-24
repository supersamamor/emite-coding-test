using Emite.Common.Core.Base.Models;
using Emite.Common.Core.Settings;
using Emite.Common.Data;
using Emite.Common.Utility.Extensions;
using Emite.Common.Utility.Models;
using LanguageExt;
using LanguageExt.ClassInstances;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Emite.Common.Core.Queries;

/// <summary>
/// A base class for queries that retrieves a single
/// record by <paramref name="Id"/>.
/// </summary>
/// <param name="Id">The Id of the record to retrieve</param>
public abstract record BaseCachedQueryById(string Id)
{
    public string GenerateCacheKey(string queryName)
    {
        return $"{queryName}_Id_{this.Id}";
    }
}

/// <summary>
/// A base class for handlers that queries the database
/// for a single record based on the primary key.
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TQuery"></typeparam>
public abstract class BaseCachedQueryByIdHandler<TContext, TEntity, TQuery, TCache>
    where TContext : DbContext
    where TEntity : BaseEntity
    where TQuery : BaseCachedQueryById
    where TCache : IMemoryCache
{
    /// <summary>
    /// An instance of TContext.
    /// </summary>
    protected readonly TContext Context;
    protected readonly TCache Cache;
    protected readonly int DefaultCacheDurationMinutes;
    /// <summary>
    /// Initializes an instance of <see cref="BaseCachedQueryByIdHandler{TContext, TEntity, TQuery}"/>.
    /// </summary>
    /// <param name="context"></param>
    public BaseCachedQueryByIdHandler(TContext context, TCache cache, ICacheSettings cacheSettings)
    {
        Context = context;
        Cache = cache;
        DefaultCacheDurationMinutes = cacheSettings.DefaultCacheDurationMinutes;
    }

    /// <summary>
    /// Retrieves a single record from the database based
    /// on the specified <paramref name="request"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<Option<TEntity>> Handle(TQuery request, CancellationToken cancellationToken = default)
    {
        var cacheKey = request.GenerateCacheKey(nameof(TQuery));
        if (Cache.TryGetValue(cacheKey, out Option<TEntity> cachedResponse))
        {
            return cachedResponse;
        }
        else
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(DefaultCacheDurationMinutes)
            };
            var response = await Context.GetSingle<TEntity>(e => e.Id == request.Id, cancellationToken: cancellationToken);
            Cache.Set(cacheKey, response, cacheEntryOptions);
            return response;
        }
    }
}
