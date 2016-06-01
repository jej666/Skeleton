using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Core.Repository;
using Skeleton.Tests.Infrastructure;
using Skeleton.Core.Service;

namespace Skeleton.Tests
{
    [TestClass]
    public class CachedServiceTests : TestBase
    {
        private readonly ICachedService<Customer, int> _service;

        public CachedServiceTests()
        {
            _service = Container.Resolve<ICachedService<Customer, int>>();

            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public void Find_ByExpression()
        {
            var sql = _service.Repository.Where(c => c.Name.Equals("Foo")).SqlQuery;
            var results = _service.Repository.Find();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_service.Repository.Cache.Contains<Customer>(
                _service.Repository.CacheKeyGenerator.ForFind(sql)));
        }

        [TestMethod]
        public void FirstOrDefault_ByExpression()
        {
            var customer1 = _service.Repository.SelectTop(1).FirstOrDefault();
            var sql = _service.Repository.Where(c => c.Name.Equals(customer1.Name)).SqlQuery;
            var customer2 = _service.Repository.FirstOrDefault();

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_service.Repository.Cache.Contains<Customer>(
                _service.Repository.CacheKeyGenerator.ForFirstOrDefault(sql)));
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            var customer1 = _service.Repository.SelectTop(1).FirstOrDefault();
            var customer2 = _service.Repository.FirstOrDefault(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_service.Repository.Cache.Contains<Customer>(
                _service.Repository.CacheKeyGenerator.ForFirstOrDefault(customer1.Id)));
        }

        [TestMethod]
        public void GetAll()
        {
            var results = _service.Repository.GetAll();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_service.Repository.Cache.Contains<Customer>(
                _service.Repository.CacheKeyGenerator.ForGetAll()));
        }
    }
}