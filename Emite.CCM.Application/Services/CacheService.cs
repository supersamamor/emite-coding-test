using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Nest;
namespace Emite.CCM.Application.Services
{
    public class CacheService
    {
        private readonly IMemoryCache _cache;
        private const string CacheKeyListKey = "CacheKeyList";
        private readonly int _defaultCacheDurationMinutes;
        private readonly int _defaultCacheKeysDurationHours = 1;
        public CacheService(IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            _cache = cache;
            _defaultCacheDurationMinutes = cacheSettings.Value.DefaultCacheDurationMinutes;
        }

        // Method to add data and track keys in Memcached
        public void SetCache(string queryName, string queryParameters, object data)
        {
            var cacheKey = $"{queryName}:{queryParameters}";
            // Add the data to the cache
            _cache.Set(cacheKey, data, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_defaultCacheDurationMinutes)
            });
            // Track cache keys in a separate Memcached entry
            var cacheKeys = GetCacheKeys();
            if (cacheKeys != null)
            {
                if (!cacheKeys.Contains(cacheKey))
                {
                    cacheKeys.Add(cacheKey);
                    _cache.Set(CacheKeyListKey, cacheKeys, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_defaultCacheKeysDurationHours)
                    });
                }
            }
        }

        // Method to remove all cache entries for a specific queryName
        public void RemoveCacheForQuery(string queryName)
        {
            var cacheKeys = GetCacheKeys();
            var keysToRemove = cacheKeys?.Where(key => key.StartsWith($"{queryName}:")).ToList();
            if (keysToRemove != null)
            {
                foreach (var key in keysToRemove)
                {
                    _cache.Remove(key);  // Remove cache entry
                    cacheKeys?.Remove(key);  // Remove key from tracking list
                }
            }
            // Update the list of cache keys in Memcached
            if (cacheKeys != null)
            {
                _cache.Set(CacheKeyListKey, cacheKeys, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_defaultCacheKeysDurationHours)
                });
            }
        }

        // Method to get all tracked cache keys from Memcached
        private List<string> GetCacheKeys()
        {
            if (_cache.TryGetValue(CacheKeyListKey, out List<string>? cacheKeyList))
            {
                if (cacheKeyList != null)
                {
                    return cacheKeyList;
                }
            }
            return [];
        }
        public TObject? GetCacheViaQueryAndParameters<TObject>(string queryName, string queryParameters)
        {
            var cacheKey = $"{queryName}:{queryParameters}";
            if (_cache.TryGetValue(cacheKey, out TObject? data))
            {
                if (data != null)
                {
                    return data;
                }
            }
            return default(TObject);
        }
        public void RemoveCache(string queryName, string queryParameters)
        {
            var cacheKey = $"{queryName}:{queryParameters}";
            _cache.Remove(cacheKey);  
        }
    }
}
