using GroceryStoreAPI.Context;
using GroceryStoreAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Services
{
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private CustomersContext _context;

        public CustomerRepository(CustomersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            try
            {
                return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch(Exception e)
            {
                throw e;
            }
            
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            try
            {
                return await _context.Customers.ToListAsync();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        public void AddCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            try
            {
                _context.Add(customer);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(IEnumerable<Guid> customerIds)
        {
            try
            {
                return await _context.Customers.Where(c => customerIds.Contains(c.Id)).ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void RemoveCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }
            try
            {
                _context.Remove(customer);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }
            try
            {
                _context.Update(customer);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
