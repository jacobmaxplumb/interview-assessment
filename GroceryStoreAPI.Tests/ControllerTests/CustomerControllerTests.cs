using AutoMapper;
using FakeItEasy;
using GroceryStoreAPI.Controllers;
using GroceryStoreAPI.Entities;
using GroceryStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Tests
{
    public class Tests
    {
        private Mock<ICustomerRepository> _customerRepo;
        private ICustomerRepository _badCustomerRepo;
        private Mock<IMapper> _mapper;
        private IEnumerable<Customer> _customers;

        [SetUp]
        public void Setup()
        {
            _customers = new List<Customer>
            {
                new Customer() { Id = Guid.NewGuid(), FirstName = "Jacob", LastName = "Plumb" },
                new Customer() { Id = Guid.NewGuid(), FirstName = "James", LastName = "Plumb" }
            };
            _customerRepo = new Mock<ICustomerRepository>();
            _badCustomerRepo = A.Fake<ICustomerRepository>();
            _mapper = new Mock<IMapper>();
            _customerRepo.Setup(x => x.GetCustomerAsync(It.IsAny<Guid>())).Returns(Task.FromResult(_customers.First()));
            _customerRepo.Setup(x => x.GetCustomersAsync()).Returns(Task.FromResult(_customers));
            A.CallTo(() => _badCustomerRepo.GetCustomerAsync(It.IsAny<Guid>())).Throws<ArgumentException>();
            A.CallTo(() => _badCustomerRepo.GetCustomersAsync()).Throws<ArgumentException>();
        }

        [Test]
        public async Task GetCustomers_Returns_OkObjectResultWithCustomersValue()
        {
            //Arrange
            var customersController = new CustomerController(_customerRepo.Object, _mapper.Object);

            //Act
            var result = await customersController.GetCustomers() as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(_customers, result.Value);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetCustomers_ReutrnsBadRequest_WhenRepoThrowsException()
        {
            //Arragne
            var customerController = new CustomerController(_badCustomerRepo, _mapper.Object);

            //Act
            var result = await customerController.GetCustomers() as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}