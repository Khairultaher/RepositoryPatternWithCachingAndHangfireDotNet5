
using CachingEnabledAPI.Models;
using CachingEnabledAPI.Repositories.Interfaces;
using CachingEnabledAPI.Services.Interfaces;
using CachingEnabledAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CachingEnabledAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository repository;
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerRepository repository
            , ICustomerService customerService)
        {
            this.repository = repository;
            this.customerService = customerService;
        }

        [HttpGet("CustomersWithOrderInfo/{skip?}/{take?}")]
        public async Task<IActionResult> CustomersWithOrderInfo(int skip = 1, int take = 0)
        {
            take = (take == 0) ? int.MaxValue : take;
            Response response = new Response();

            var data = await customerService.CustomersWithOrderInfo(skip, take);
            response.Data = data; response.TotalRecords = data.Count;
            return Ok(response);
        }


        [HttpGet("CustomersHasOrder/{skip?}/{take?}")]
        public async Task<IActionResult> GetCustomersHasOrder(int skip = 1, int take = 0)
        {
            take = (take == 0) ? int.MaxValue : take;
            Response response = new Response();

            var data = await customerService.GetCustomersHasOrder(skip, take);
            response.Data = data; response.TotalRecords = data.Count;
            return Ok(response);
        }

        [HttpGet("CustomersHasNoOrder/{count}/{skip?}/{take?}")]
        public async Task<IActionResult> GetCustomersHasNoOrder(int count, int skip = 1, int take = 0)
        {
            take = (take == 0) ? int.MaxValue : take;
            Response response = new Response();

            var data = await customerService.GetCustomersHasNoOrder(skip, take);
            response.Data = data; response.TotalRecords = data.Count;
            return Ok(response);
        }


        [HttpGet("OrdersWithCustomerInfo/{skip?}/{take?}")]
        public async Task<IActionResult> GetOrdersWithCustomerInfo(int skip = 1, int take = 0)
        {
            take = (take == 0) ? int.MaxValue : take;
            Response response = new Response();

            var data = await customerService.GetOrdersWithCustomerInfo(skip, take);
            response.Data = data; response.TotalRecords = data.Count;
            return Ok(response);
        }

        [HttpPost("refreshcustomerscache")]
        public async Task<IActionResult> RefreshCustomersCache([FromForm] string cacheKey)
        {
            await repository.RefreshCustomersCache();
            return Ok();
        }

        [HttpGet]
        public async Task<IReadOnlyList<Customer>> Get()
        {
            return await repository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
        {
            var customer = await repository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }
            await repository.UpdateAsync(customer);
            return NoContent();
        }
       
        [HttpPost]
        public async Task<ActionResult<Customer>> Post([FromForm]Customer customer)
        {
            await repository.AddAsync(customer);
            return CreatedAtAction("Get", new { id = customer.Id }, customer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            var customer = await repository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await repository.DeleteAsync(customer);
            return customer;
        }
    }
}
