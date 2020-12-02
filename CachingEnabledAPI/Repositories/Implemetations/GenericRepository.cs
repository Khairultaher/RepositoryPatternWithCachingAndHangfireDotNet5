using Hangfire;
using Microsoft.EntityFrameworkCore;
using CachingEnabledAPI.Models;
using CachingEnabledAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CachingEnabledAPI.Repositories.Interfaces;
using CachingEnabledAPI.Services.Interfaces;

namespace CachingEnabledAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : Auditable
    {
        private readonly static CacheTech cacheTech = CacheTech.Redis;
        // private readonly string cacheKey = $"{typeof(T)}";
        private readonly string cacheKey = $"Customers";
        private readonly ApplicationDbContext _dbContext;
        private readonly Func<CacheTech, ICacheService> _cacheService;
        public GenericRepository(ApplicationDbContext dbContext, Func<CacheTech, ICacheService> cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<List<T>> GetAllAsync()
        {
            if (!_cacheService(cacheTech).TryGet(cacheKey, out List<T> cachedList))
            {
                cachedList = await _dbContext.Set<T>().OrderByDescending(o => o.Id).ToListAsync();
                _cacheService(cacheTech).Set(cacheKey, cachedList);
            }
            return cachedList;
        }
        public IQueryable<T> GetAll()
        {
            //var ddd = _dbContext.Customers; 
            return _dbContext.Set<T>().AsNoTracking();
        }
        public async Task RefreshCache()
        {
            var cachedList = await _dbContext.Set<T>().OrderByDescending(o => o.Id).ToListAsync();
            //_cacheService(cacheTech).Remove(cacheKey);
            _cacheService(cacheTech).Set(cacheKey, cachedList);
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());
            return entity;
        }
        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());
        }
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task RefreshCustomersCache()
        {
            await Task.Delay(100);
            BackgroundJob.Enqueue(() => RefreshCache());
        }
    }
}
