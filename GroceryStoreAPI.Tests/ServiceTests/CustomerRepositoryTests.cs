using GroceryStoreAPI.Context;
using GroceryStoreAPI.Entities;
using GroceryStoreAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Tests.ServiceTests
{
    public class CustomerRepositoryTests
    {
        private CustomersContext _context;
        private List<Customer> _customers;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CustomersContext>()
                .UseInMemoryDatabase(databaseName: "MovieListDatabase")
                .Options;
            _context = new CustomersContext(options);
            _customers = new List<Customer>()
            {
                new Customer { Id = Guid.NewGuid(), FirstName = "Jacob", LastName = "Plumb" },
                new Customer { Id = Guid.NewGuid(), FirstName = "James", LastName = "Plumb" }
            };
            _context.AddRange(_customers);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context = null;
        }


        [Test]
        public async Task GetCustomersAsync_Returns_ListOfCustomers()
        {
            //Arrange
            var sut = new CustomerRepository(_context);

            //Act
            var customers = await sut.GetCustomersAsync();

            //Assert  
            Assert.NotNull(customers);
            Assert.AreEqual(_customers.Count(), customers.Count());
            Assert.AreEqual(_customers, customers);
        }

        [Test]
        public async Task GetCustomerAsync_Returns_OneCustomer()
        {
            //Arrange
            var sut = new CustomerRepository(_context);

            //Act
            var customer = await sut.GetCustomerAsync(_customers.First().Id);

            //Assert
            Assert.NotNull(customer);
            Assert.AreEqual(_customers.First(), customer);

        }
    }
}
