﻿using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class CrudRepositoryTests : TestBase
    {
        private readonly ICrudRepository<Customer, int, CustomerDto> _repository;

        public CrudRepositoryTests()
        {
            _repository = Container.Resolve<ICrudRepository<Customer, int, CustomerDto>>();

            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public void Add()
        {
            var customer = new Customer {Name = "Customer"};
            var successed = _repository.Store.Add(customer);
            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id > 0);

            var result = _repository.Query.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void Add_Multiple()
        {
            var customers = MemorySeeder.SeedCustomers(5);
            var successed = _repository.Store.Add(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void Delete()
        {
            var customer = _repository.Query
                .Top(1)
                .FirstOrDefault();
            var successed = _repository.Store.Delete(customer);
            Assert.IsTrue(successed);

            var result = _repository.Query.FirstOrDefault(customer.Id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Delete_Multiple()
        {
            var customers = _repository.Query
                .Top(3)
                .Find();
            var successed = _repository.Store.Delete(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void Update()
        {
            var customer = _repository.Query
                .Top(1)
                .FirstOrDefault();
            customer.Name = "CustomerUpdated";
            var successed = _repository.Store.Update(customer);
            Assert.IsTrue(successed);

            var result = _repository.Query.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Name.Equals("CustomerUpdated"));
        }

        [TestMethod]
        public void Update_Multiple()
        {
            var customers = _repository.Query
                .Top(5)
                .Find()
                .ToList();
            foreach (var cust in customers)
                cust.Name = "CustomerUpdated" + cust.Id;

            var successed = _repository.Store.Update(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void Save_ShouldAdd()
        {
            var customer = new Customer {Name = "Customer"};
            var successed = _repository.Store.Save(customer);
            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id > 0);

            var result = _repository.Query.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void Save_ShouldUpdate()
        {
            var customer = _repository.Query.Top(1).FirstOrDefault();
            Assert.IsTrue(customer.Id > 0);

            customer.Name = "CustomerUpdated";
            var successed = _repository.Store.Save(customer);
            Assert.IsTrue(successed);

            var result = _repository.Query.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Name.Equals("CustomerUpdated"));
        }

        [TestMethod]
        public void Save_Multiple()
        {
            var customers = MemorySeeder.SeedCustomers(5);
            var successed = _repository.Store.Save(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void Dispose_Store()
        {
            using (_repository.Store)
            {
            }

            var fieldInfo = typeof(DisposableBase).GetField("_disposed",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(fieldInfo);
            Assert.IsTrue((bool)fieldInfo.GetValue(_repository.Store));
        }
    }
}