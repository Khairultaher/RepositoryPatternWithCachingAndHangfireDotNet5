using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using CachingEnabledAPI.Configurations;
using CachingEnabledAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CachingEnabledAPI.Services.Interfaces;

namespace CachingEnabledAPI.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache memoryCache;
        private readonly CacheConfiguration cacheConfig;
        private MemoryCacheEntryOptions cacheOptions;
        public MemoryCacheService(IMemoryCache memoryCache, IOptions<CacheConfiguration> cacheConfig)
        {
            this.memoryCache = memoryCache;
            this.cacheConfig = cacheConfig.Value;
            if (this.cacheConfig != null)
            {
                this.cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(this.cacheConfig.AbsoluteExpirationInHours),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(this.cacheConfig.SlidingExpirationInMinutes)
                };
            }
        }
        public bool TryGet<T>(string cacheKey, out T value)
        {
            this.memoryCache.TryGetValue(cacheKey, out value);
            if (value == null) return false;
            else return true;
        }
        public void Set<T>(string cacheKey, T value)
        {
            this.memoryCache.Set(cacheKey, value, this.cacheOptions);
            return;
        }
        public void Remove(string cacheKey)
        {
            this.memoryCache.Remove(cacheKey);
        }
    }
}
