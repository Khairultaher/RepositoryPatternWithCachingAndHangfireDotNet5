
using CachingEnabledAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CachingEnabledAPI.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerVM>> CustomersWithOrderInfo(int skip, int take);
        Task<List<OrderVM>> GetOrdersWithCustomerInfo(int skip, int take);
        Task<List<CustomerVM>> GetCustomersHasOrder(int skip, int take);
        Task<List<CustomerVM>> GetCustomersHasNoOrder(int skip, int take);
    }
}
