using CachingEnabledAPI.Data;
using CachingEnabledAPI.Models;
using CachingEnabledAPI.Repositories.Interfaces;
using CachingEnabledAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CachingEnabledAPI.Repositories.Implemetations
{
    public class OrderRepository : GenericRepository<Order>
        , IOrderRepository
    {
        private readonly DbSet<Order> order;
        public OrderRepository(ApplicationDbContext dbContext
            , Func<CacheTech, ICacheService> cacheService)
            : base(dbContext, cacheService)
        {
            this.order = dbContext.Set<Order>();
        }
    }
}
