using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Core.Repository;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class CachedRepositoryAsyncTests : TestBase
    {
        private readonly ICachedRepositoryAsync<Customer, int> _repository;

        public CachedRepositoryAsyncTests()
        {
            _repository = Container.Resolve<ICachedRepositoryAsync<Customer, int>>();
        }

        [TestMethod]
        public async Task FindAsync_ByExpression()
        {
            var sql = _repository.Where(c => c.Name.Equals("Foo")).SqlQuery;
            var results = await _repository.FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Cache.Contains<Customer>(
                _repository.CacheKeyGenerator.ForFind(sql)));
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
                _repository.CacheKeyGenerator.ForFirstOrDefault(sql)));
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
                _repository.CacheKeyGenerator.ForFirstOrDefault(customer1.Id)));
        }

        [TestMethod]
        public async Task GetAllAsync()
        {
            var results = await _repository.GetAllAsync();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Cache.Contains<Customer>(
                _repository.CacheKeyGenerator.ForGetAll()));
        }
    }
}