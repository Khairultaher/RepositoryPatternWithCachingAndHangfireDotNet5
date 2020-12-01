using CachingEnabledAPI.Repositories.Interfaces;
using CachingEnabledAPI.Services.Interfaces;
using CachingEnabledAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CachingEnabledAPI.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IOrderRepository orderRepository;
        public CustomerService(ICustomerRepository customerRepository
            , IOrderRepository orderRepository)
        {
            this.customerRepository = customerRepository;
            this.orderRepository = orderRepository;
        }

        //.Skip((skip - 1) * take).Take(take)
        public async Task<List<OrderVM>> GetOrdersWithCustomerInfo(int skip, int take)
        {
            var query = from o in orderRepository.GetAll()
                        join c in customerRepository.GetAll() // First approach
                             on o.CustomerId equals c.Id


                        //from c in customerRepository.GetAll() // Second approach
                        //                         .Where(w => w.Id == o.CustomerId)
                        
                        select new OrderVM
                        {
                            Id = o.Id,
                            Quantity = o.Quantity,
                            Price = o.Price,
                            Total = o.Total,
                            OrderDate = o.OrderDate,
                            CustomerId = o.CustomerId,
                            CustomerName = c.FirstName,
                            CustomerEmail = c.Email
                        };
                  
            return await Task.FromResult(query.ToList());
            
        }

        public async Task<List<CustomerVM>> GetCustomersHasOrder(int skip, int take)
        {
            var query = from c in customerRepository.GetAll()
                        join ord in orderRepository.GetAll()  // First approach
                              on c.Id equals ord.CustomerId
                        group ord by new
                        {
                            c.Id,
                            c.FirstName,
                            c.LastName,
                            c.Contact,
                            c.Email,
                            c.DateOfBirth
                        } into grp
                        where grp.Count() > 0
                        select new CustomerVM
                        {
                            Id = grp.Key.Id,
                            FirstName = grp.Key.FirstName,
                            LastName = grp.Key.LastName,
                            Contact = grp.Key.Contact,
                            Email = grp.Key.Email,
                            DateOfBirth = grp.Key.DateOfBirth,
                        };

            return await Task.FromResult(query.ToList());
        }

        public async Task<List<CustomerVM>> GetCustomersHasNoOrder(int skip, int take)
        {
            //await Task.Delay(100);

            var query = from c in customerRepository.GetAll()
                        join ord in orderRepository.GetAll()  // First approach
                               on c.Id equals ord.CustomerId into grp
                        from o in grp.DefaultIfEmpty()

                            //from o in orderRepository.GetAll()  // Second approach
                            //                         .Where(w => w.CustomerId == c.Id)
                            //                         .DefaultIfEmpty()

                        where o.Id == null
                        select new CustomerVM
                        {
                            Id = c.Id,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Contact = c.Contact,
                            Email = c.Email,
                            DateOfBirth = c.DateOfBirth

                        };

            //return query.ToList();
            return await Task.FromResult(query.ToList());
        }

        public async Task<List<CustomerVM>> CustomersWithOrderInfo(int skip, int take)
        {
            var query = from c in customerRepository.GetAll()
                        from o in orderRepository.GetAll()
                                                 .Where(w => w.CustomerId == c.Id)
                                                 .DefaultIfEmpty()
                        select new CustomerVM
                        {
                            Id = c.Id,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Contact = c.Contact,
                            Email = c.Email,
                            DateOfBirth = c.DateOfBirth,
                            OrderId = o.Id,
                            OrderDate = o.OrderDate                   
                        };

            return await Task.FromResult(query.ToList());
        }
        
    }
}
