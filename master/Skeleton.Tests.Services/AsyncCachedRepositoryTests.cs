using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Tests.Common;
using System;

namespace Skeleton.Tests.Services
{
    [TestClass]
    public class AsyncCachedRepositoryTests : RepositoryTestBase
    {
        private readonly IAsyncCachedReadRepository<Customer, CustomerDto> _repository;

        public AsyncCachedRepositoryTests()
        {
            _repository = Container.Resolve<IAsyncCachedReadRepository<Customer, CustomerDto>>();
        }

        [TestMethod]
        public async Task Cached_FindAsync_ByExpression()
        {
            var results = await _repository.Query
                .Where(c => c.Name.StartsWith("Customer", StringComparison.InvariantCultureIgnoreCase))
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Query.Cache.Contains(
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
            Assert.IsTrue(_repository.Query.Cache.Contains(
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
            Assert.IsTrue(_repository.Query.Cache.Contains(
                _repository.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public async Task Cached_GetAllAsync()
        {
            var results = await _repository.Query.FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_repository.Query.Cache.Contains(
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

                Assert.IsTrue(results.Count() <= pageSize);
                Assert.IsTrue(_repository.Query.Cache.Contains(
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