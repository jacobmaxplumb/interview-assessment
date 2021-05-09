
using GroceryStoreAPI.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Services
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerAsync(Guid id);
        void AddCustomer(Entities.Customer customer);
        void RemoveCustomer(Entities.Customer customer);
        void UpdateCustomer(Entities.Customer customer);

        Task<IEnumerable<Entities.Customer>> GetCustomersAsync(IEnumerable<Guid> customerIds);
        Task<bool> SaveChangesAsync();
    }
}
