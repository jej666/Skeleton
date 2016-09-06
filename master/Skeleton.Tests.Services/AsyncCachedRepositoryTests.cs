using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class AsyncCachedRepositoryTests : TestBase
    {
        private readonly IAsyncCachedReadRepository<Customer, int, CustomerDto> _repository;

        public AsyncCachedRepositoryTests()
        {
            _repository = Container.Resolve<IAsyncCachedReadRepository<Customer, int, CustomerDto>>();
        }

        [TestMethod]
        public async Task Cached_FindAsync_ByExpression()
        {
            var results = await _repository.Query
                .Where(c => c.Name.StartsWith("Customer"))
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Query.Cache.Contains<Customer>(
                _repository.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public async Task Cached_FirstOrDefaultAsync_ByExpression()
        {
            var customer1 = await _repository.Query
                .Top(1)
                .FirstOrDefaultAsync();
            var customer2 = await _repository.Query
                .Where(c => c.Name.Equals(customer1.Name))
                .FirstOrDefaultAsync();

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_repository.Query.Cache.Contains<Customer>(
                _repository.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public async Task Cached_FirstOrDefaultAsync_ById()
        {
            var customer1 = await _repository.Query
                .Top(1)
                .FirstOrDefaultAsync();
            var customer2 = await _repository.Query
                .FirstOrDefaultAsync(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_repository.Query.Cache.Contains<Customer>(
                _repository.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public async Task Cached_GetAllAsync()
        {
            var results = await _repository.Query.GetAllAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Query.Cache.Contains<Customer>(
                _repository.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public async Task Cached_PageAsync()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var results = await _repository.Query
                    .OrderBy(c => c.CustomerCategoryId)
                    .PageAsync(pageSize, page);

                Assert.AreEqual(pageSize, results.Count());
                Assert.IsTrue(_repository.Query.Cache.Contains<Customer>(
                    _repository.Query.LastGeneratedCacheKey));
            }
        }

        [TestMethod]
        public void Dispose_Query()
        {
            using (_repository.Query)
            {
            }

            var fieldInfo = typeof(DisposableBase).GetField("_disposed",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(fieldInfo);
            Assert.IsTrue((bool)fieldInfo.GetValue(_repository.Query));
        }
    }
}