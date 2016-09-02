using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Core.Repository;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class CachedRepositoryTests : TestBase
    {
        private readonly ICachedReadRepository<Customer, int, CustomerDto> _service;

        public CachedRepositoryTests()
        {
            _service = Container.Resolve<ICachedReadRepository<Customer, int, CustomerDto>>();
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
                .Top(1)
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
            var customer1 = _service.Query.Top(1).FirstOrDefault();
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

        [TestMethod]
        public void Cached_Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var results = _service.Query
                    .OrderBy(c => c.CustomerCategoryId)
                    .Page(pageSize, page);

                Assert.AreEqual(pageSize, results.Count());
                Assert.IsTrue(_service.Query.Cache.Contains<Customer>(
                    _service.Query.LastGeneratedCacheKey));
            }
        }
    }
}