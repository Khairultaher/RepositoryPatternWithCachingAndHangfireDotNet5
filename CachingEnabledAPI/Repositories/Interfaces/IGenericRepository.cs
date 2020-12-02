using CachingEnabledAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CachingEnabledAPI.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : Auditable
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        IQueryable<T> GetAll();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task RefreshCustomersCache();
    }
}
