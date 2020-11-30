using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CachingEnabledAPI.Services.Interfaces
{
    public interface ICacheService
    {
        bool TryGet<T>(string cacheKey, out T value);
        void Set<T>(string cacheKey, T value);
        void Remove(string cacheKey);
    }
}
