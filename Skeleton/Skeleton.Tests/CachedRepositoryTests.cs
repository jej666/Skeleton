using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Common;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Tests.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Tests
{
    [TestClass]
    public class CachedRepositoryTests : TestBase
    {
        private readonly CachedCustomerRepository repository;

        public CachedRepositoryTests()
        {
            var accessorCache = Container.Resolve<ITypeAccessorCache>();
            var database = Container.Resolve<IDatabase>();
            var cacheProvider = Container.Resolve<ICacheProvider>();
            repository = new CachedCustomerRepository(accessorCache, cacheProvider, database);
        }

        [TestMethod]
        public void Find_ByExpression()
        {
            var results = repository.Find(
                c => c.Name.Equals("Kevin"),
                c => c.CustomerId);
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForFind(repository.Query.AsSql())));
        }

        [TestMethod]
        public void FirstOrDefault_ByExpression()
        {
            var customer1 = repository.FirstOrDefault(c => c.Name.Equals("Foo"));
            Assert.IsNotNull(customer1);
            Assert.IsInstanceOfType(customer1, typeof(Customer));

            var customer2 = repository.FirstOrDefault(c => c.Name.Equals("Foo"));
            Assert.AreEqual(customer1, customer2);
            var parameters = new Dictionary<string, object>() { { "P1", "Foo" } };
            Assert.IsTrue(repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForFirstOrDefault(parameters)));
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            var customer1 = repository.Query.SelectTop(1).FirstOrDefault();
            var customer2 = repository.FirstOrDefault(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForFirstOrDefault(customer1.Id)));
        }

        [TestMethod]
        public void GetAll()
        {
            var results = repository.GetAll();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForGetAll()));
        }
    }
}