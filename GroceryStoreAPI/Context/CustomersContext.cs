using GroceryStoreAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace GroceryStoreAPI.Context
{
    public class CustomersContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomersContext() { }

        public CustomersContext(DbContextOptions<CustomersContext> options) : base(options) { }
    }
}
