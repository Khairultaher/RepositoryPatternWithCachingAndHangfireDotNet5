
using CachingEnabledAPI.Models;
using CachingEnabledAPI.Repositories.Interfaces;
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
        public CustomerController(ICustomerRepository repository)
        {
            this.repository = repository;
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
