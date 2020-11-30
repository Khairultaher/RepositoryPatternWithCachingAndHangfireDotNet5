using Microsoft.EntityFrameworkCore;
using CachingEnabledAPI.Data;
using CachingEnabledAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CachingEnabledAPI.Repositories.Interfaces;
using CachingEnabledAPI.Services.Interfaces;

namespace CachingEnabledAPI.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, 
        ICustomerRepository
    {
        private readonly DbSet<Customer> customer;
        public CustomerRepository(ApplicationDbContext dbContext
            , Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
            this.customer = dbContext.Set<Customer>();
        }
    }
}
