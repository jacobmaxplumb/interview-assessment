using AutoMapper;
using GroceryStoreAPI.Filters;
using GroceryStoreAPI.ModelBinders;
using GroceryStoreAPI.Models;
using GroceryStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    [ApiController]
    [Route("api/customerscollection")]
    public class CustomersCollectionController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomersCollectionController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        [HttpGet("({customerIds})", Name = "GetCustomerCollection")]
        [CustomersResultFilter]
        public async Task<IActionResult> GetCustomerCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> customerIds)
        {
            var customerEntities = await _customerRepository.GetCustomersAsync(customerIds);
            if (customerIds.Count() != customerEntities.Count())
            {
                return NotFound();
            }
            return Ok(customerEntities);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerCollection(IEnumerable<CustomerForCreation> customersCollection)
        {
            var customerEntities = _mapper.Map<IEnumerable<Entities.Customer>>(customersCollection);
            foreach (var customerEntity in customerEntities)
            {
                _customerRepository.AddCustomer(customerEntity);
            }
            await _customerRepository.SaveChangesAsync();
            var customersToReturn = await _customerRepository.GetCustomersAsync(customerEntities.Select(c => c.Id).ToList());

            var customerIds = string.Join(",", customersToReturn.Select(c => c.Id));
            return CreatedAtAction("GetCustomerCollection", new { customerIds }, customersToReturn);
        }
    }
}
