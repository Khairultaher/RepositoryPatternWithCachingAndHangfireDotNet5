using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using CachingEnabledAPI.Configurations;
using CachingEnabledAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CachingEnabledAPI.Services.Interfaces;

namespace CachingEnabledAPI.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache distributedCache;
        private readonly CacheConfiguration cacheConfig;
        private DistributedCacheEntryOptions cacheOptions;
        public RedisCacheService(IDistributedCache distributedCache, IOptions<CacheConfiguration> cacheConfig)
        {
            this.distributedCache = distributedCache;
            this.cacheConfig = cacheConfig.Value;

            this.cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(this.cacheConfig.AbsoluteExpirationInHours),
                SlidingExpiration = TimeSpan.FromMinutes(this.cacheConfig.SlidingExpirationInMinutes)
            };
        }
        public void Remove(string cacheKey)
        {
            distributedCache.Remove(cacheKey);
        }
        public void Set<T>(string cacheKey, T value)
        {
            var serializedData = JsonConvert.SerializeObject(value);
            var encodedData = Encoding.UTF8.GetBytes(serializedData);
            distributedCache.Set(cacheKey, encodedData, cacheOptions);
        }
        public bool TryGet<T>(string cacheKey, out T value)
        {
            value = default;
            var data =  distributedCache.Get(cacheKey);
            string serializedData = "";
            
            if (data != null)
            {
                serializedData = Encoding.UTF8.GetString(data);
                value = JsonConvert.DeserializeObject<T>(serializedData);
                return true;
            }
            return false;
        }
    }
}
