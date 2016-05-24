using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Common;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Tests.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.Tests
{
    [TestClass]
    public class CachedRepositoryAsyncTests : TestBase
    {
        private readonly CachedCustomerRepositoryAsync _repository;

        public CachedRepositoryAsyncTests()
        {
            var accessorCache = Container.Resolve<ITypeAccessorCache>();
            var database = Container.Resolve<IDatabaseAsync>();
            var cacheProvider = Container.Resolve<ICacheProvider>();

            _repository = new CachedCustomerRepositoryAsync(
                accessorCache, cacheProvider, database);
        }

        [TestMethod]
        public async Task FindAsync_ByExpression()
        {
            var sql = _repository.Where(c => c.Name.Equals("Foo")).SqlQuery;
            var results = await _repository.FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForFind(sql)));
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ByExpression()
        {
            var customer1 = await _repository.SelectTop(1).FirstOrDefaultAsync();
            var sql = _repository.Where(c => c.Name.Equals(customer1.Name)).SqlQuery;
            var customer2 = await _repository.FirstOrDefaultAsync();

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForFirstOrDefault(sql)));
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ById()
        {
            var customer1 = await _repository.SelectTop(1).FirstOrDefaultAsync();
            var customer2 = await _repository.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForFirstOrDefault(customer1.Id)));
        }

        [TestMethod]
        public async Task GetAllAsync()
        {
            var results = await _repository.GetAllAsync();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Cache.Contains<Customer>(
                CustomerCacheKey.ForGetAll()));
        }
    }
}

