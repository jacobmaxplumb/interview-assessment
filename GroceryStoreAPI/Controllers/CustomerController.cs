using AutoMapper;
using GroceryStoreAPI.Filters;
using GroceryStoreAPI.Models;
using GroceryStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [CustomersResultFilter]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                return Ok(await _customerRepository.GetCustomersAsync());
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
            
        }

        [HttpGet]
        [Route("{id}", Name = "GetCustomer")]
        [CustomerResultFilter]

        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(await _customerRepository.GetCustomerAsync(id));
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
            
        }

        [HttpPost]
        [CustomerResultFilter]
        public async Task<IActionResult> CreateCustomer(CustomerForCreation customerForCreation)
        {
            var customerEntity = _mapper.Map<Entities.Customer>(customerForCreation);
            if (customerEntity.FirstName == null || customerEntity.LastName == null)
            {
                return BadRequest(new { error = "FirstName or LastName cannot be empty" });
            }
            _customerRepository.AddCustomer(customerEntity);

            await _customerRepository.SaveChangesAsync();

            var addedCustomer = await _customerRepository.GetCustomerAsync(customerEntity.Id);

            return CreatedAtRoute("GetCustomer", new { id = addedCustomer.Id }, addedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetCustomerAsync(id);
            if (customer == null)
            {
                return BadRequest(new { error = $"Could not find customer with id of {id}" });
            }
            _customerRepository.RemoveCustomer(customer);
            await _customerRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        [CustomerResultFilter]
        public async Task<IActionResult> UpdateCustomer(Guid id, CustomerForCreation customerForCreation)
        {
            var customerEntity = _mapper.Map<Entities.Customer>(customerForCreation);
            customerEntity.Id = id;
            _customerRepository.UpdateCustomer(customerEntity);
            await _customerRepository.SaveChangesAsync();
            return Ok(customerEntity);
        }
    }
}
