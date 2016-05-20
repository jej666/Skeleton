using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Common;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class CachedRepositoryTests : TestBase
    {
        private readonly CachedCustomerRepository _repository;

        public CachedRepositoryTests()
        {
            var accessorCache = Container.Resolve<ITypeAccessorCache>();
            var database = Container.Resolve<IDatabase>();
            var cacheProvider = Container.Resolve<ICacheProvider>();
            _repository = new CachedCustomerRepository(
                accessorCache, cacheProvider, database);
        }

        [TestMethod]
        public void Find_ByExpression()
        {
            var sql = _repository.Where(c => c.Name.Equals("Foo")).SqlQuery;
            var results = _repository.Find(); 
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForFind(sql)));
        }

        [TestMethod]
        public void FirstOrDefault_ByExpression()
        {
            var customer1 = _repository.Where(c => c.Name.Equals("Foo"))
                                       .FirstOrDefault();
            Assert.IsNotNull(customer1);
            Assert.IsInstanceOfType(customer1, typeof(Customer));

            var customer2 = _repository.Where(c => c.Name.Equals("Foo"))
                                       .FirstOrDefault();
            Assert.AreEqual(customer1, customer2);
            var parameters = new Dictionary<string, object> { { "P1", "Foo" } };
            Assert.IsTrue(_repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForFirstOrDefault(parameters)));
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            var customer1 = _repository.SelectTop(1).FirstOrDefault();
            var customer2 = _repository.FirstOrDefault(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForFirstOrDefault(customer1.Id)));
        }

        [TestMethod]
        public void GetAll()
        {
            var results = _repository.GetAll();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForGetAll()));
        }
    }
}