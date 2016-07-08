using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Tests.Infrastructure;
using Skeleton.Core.Service;

namespace Skeleton.Tests
{
    [TestClass]
    public class CachedServiceAsyncTests : TestBase
    {
        private readonly ICachedServiceAsync<Customer, int> _service;

        public CachedServiceAsyncTests()
        {
            _service = Container.Resolve<ICachedServiceAsync<Customer, int>>();
        }

        [TestMethod]
        public async Task FindAsync_ByExpression()
        {
            var sql = _service.Repository.Where(c => c.Name.StartsWith("Customer")).SqlQuery;
            var results = await _service.Repository.FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_service.Repository.Cache.Contains<Customer>(
                _service.Repository.CacheKeyGenerator.ForFind(sql)));
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ByExpression()
        {
            var customer1 = await _service.Repository.SelectTop(1).FirstOrDefaultAsync();
            var sql = _service.Repository.Where(c => c.Name.Equals(customer1.Name)).SqlQuery;
            var customer2 = await _service.Repository.FirstOrDefaultAsync();

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_service.Repository.Cache.Contains<Customer>(
                _service.Repository.CacheKeyGenerator.ForFirstOrDefault(sql)));
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ById()
        {
            var customer1 = await _service.Repository.SelectTop(1).FirstOrDefaultAsync();
            var customer2 = await _service.Repository.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_service.Repository.Cache.Contains<Customer>(
                _service.Repository.CacheKeyGenerator.ForFirstOrDefault(customer1.Id)));
        }

        [TestMethod]
        public async Task GetAllAsync()
        {
            var results = await _service.Repository.GetAllAsync();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_service.Repository.Cache.Contains<Customer>(
                _service.Repository.CacheKeyGenerator.ForGetAll()));
        }
    }
}