using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class CachedRepositoryTests : TestBase
    {
        private readonly ICachedReadRepository<Customer, CustomerDto> _repository;

        public CachedRepositoryTests()
        {
            _repository = Container.Resolve<ICachedReadRepository<Customer, CustomerDto>>();
            _repository.Query.CacheConfigurator = config => config.SetAbsoluteExpiration(TimeSpan.FromSeconds(300));
            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public void Cached_Find_ByExpression()
        {
            var results = _repository.Query
                .Where(c => c.Name.StartsWith("Customer"))
                .Find();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Query.Cache.Contains<Customer>(
                _repository.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public void Cached_FirstOrDefault_ByExpression()
        {
            var customer1 = _repository.Query
                .Top(1)
                .FirstOrDefault();
            var customer2 = _repository.Query
                .Where(c => c.Name.Equals(customer1.Name))
                .FirstOrDefault();

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_repository.Query.Cache.Contains<Customer>(
                _repository.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public void Cached_FirstOrDefault_ById()
        {
            var customer1 = _repository.Query.Top(1).FirstOrDefault();
            var customer2 = _repository.Query.FirstOrDefault(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_repository.Query.Cache.Contains<Customer>(
                _repository.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public void Cached_GetAll()
        {
            var results = _repository.Query.GetAll();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Query.Cache.Contains<Customer>(
                _repository.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public void Cached_Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var results = _repository.Query
                    .OrderBy(c => c.CustomerCategoryId)
                    .Page(pageSize, page);

                Assert.AreEqual(pageSize, results.Count());
                Assert.IsTrue(_repository.Query.Cache.Contains<Customer>(
                    _repository.Query.LastGeneratedCacheKey));
            }
        }

        [TestMethod]
        public void Cached_Dispose_Query()
        {
            using (_repository.Query)
            {
            }

            var fieldInfo = typeof(DisposableBase).GetField("_disposed",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(fieldInfo);
            Assert.IsTrue((bool) fieldInfo.GetValue(_repository.Query));
        }
    }
}