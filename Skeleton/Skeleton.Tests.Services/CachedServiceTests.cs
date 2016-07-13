using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Core.Service;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class CachedServiceTests : TestBase
    {
        private readonly ICachedReadService<Customer, int, CustomerDto> _service;

        public CachedServiceTests()
        {
            _service = Container.Resolve<ICachedReadService<Customer, int, CustomerDto>>();
            _service.Query.CacheConfigurator = config => config.SetAbsoluteExpiration(TimeSpan.FromSeconds(300));
            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public void Cached_Find_ByExpression()
        {
            var results = _service.Query
                .Where(c => c.Name.StartsWith("Customer"))
                .Find();
            
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_service.Query.Cache.Contains<Customer>(
               _service.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public void Cached_FirstOrDefault_ByExpression()
        {
            var customer1 = _service.Query
                .SelectTop(1)
                .FirstOrDefault();
            var customer2 = _service.Query
                .Where(c => c.Name.Equals(customer1.Name))
                .FirstOrDefault();

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_service.Query.Cache.Contains<Customer>(
                _service.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public void Cached_FirstOrDefault_ById()
        {
            var customer1 = _service.Query.SelectTop(1).FirstOrDefault();
            var customer2 = _service.Query.FirstOrDefault(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_service.Query.Cache.Contains<Customer>(
                _service.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public void Cached_GetAll()
        {
            var results = _service.Query.GetAll();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_service.Query.Cache.Contains<Customer>(
                _service.Query.LastGeneratedCacheKey));
        }
    }
}