using Emite.Common.Core.Settings;
using Emite.Common.Utility.Extensions;
using Emite.Common.Utility.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Emite.Common.Core.Queries;

/// <summary>
/// A base class for query handlers that retrieve
/// records from a database.
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TQuery"></typeparam>
public abstract class BaseCachedQueryHandler<TContext, TEntity, TQuery, TCache>
    where TContext : DbContext
    where TEntity : class
    where TQuery : BaseQuery
    where TCache : IMemoryCache
{
    /// <summary>
    /// An instance of TContext.
    /// </summary>
    protected readonly TContext Context;
    protected readonly TCache Cache;
    protected readonly int DefaultCacheDurationMinutes;
    /// <summary>
    /// Initializes an instance of <see cref="BaseCachedQueryHandler{TContext, TEntity, TQuery}"/>.
    /// </summary>
    /// <param name="context"></param>
    public BaseCachedQueryHandler(TContext context, TCache cache, ICacheSettings cacheSettings)
    {
        Context = context;
        Cache = cache;
        DefaultCacheDurationMinutes = cacheSettings.DefaultCacheDurationMinutes;
    }

    /// <summary>
    /// Retrieves records from the database based on the
    /// specified <paramref name="request"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task<PagedListResponse<TEntity>> Handle(TQuery request, CancellationToken cancellationToken = default)
    {
        if (Cache.TryGetValue(nameof(TQuery), out PagedListResponse<TEntity>? cachedResponse))
        {
            return Task.FromResult(cachedResponse!);
        }
        else
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(DefaultCacheDurationMinutes)
            };
            var response = Task.FromResult(Context.Set<TEntity>().AsNoTracking().ToPagedResponse(request.SearchColumns, request.SearchValue,
                                                                request.SortColumn, request.SortOrder,
                                                                request.PageNumber, request.PageSize));
            Cache.Set(nameof(TQuery), response, cacheEntryOptions);
            return response;
        }
    }

}
