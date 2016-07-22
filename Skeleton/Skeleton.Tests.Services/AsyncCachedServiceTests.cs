using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Core.Service;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class AsyncCachedServiceTests : TestBase
    {
        private readonly IAsyncCachedReadService<Customer, int,CustomerDto> _readService;

        public AsyncCachedServiceTests()
        {
            _readService = Container.Resolve<IAsyncCachedReadService<Customer, int, CustomerDto>>();
        }

        [TestMethod]
        public async Task Cached_FindAsync_ByExpression()
        {
            var results = await _readService.Query
                .Where(c => c.Name.StartsWith("Customer"))
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_readService.Query.Cache.Contains<Customer>(
                _readService.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public async Task Cached_FirstOrDefaultAsync_ByExpression()
        {
            var customer1 = await _readService.Query
                .Top(1)
                .FirstOrDefaultAsync();
            var customer2 = await _readService.Query
                .Where(c => c.Name.Equals(customer1.Name))
                .FirstOrDefaultAsync();

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_readService.Query.Cache.Contains<Customer>(
                _readService.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public async Task Cached_FirstOrDefaultAsync_ById()
        {
            var customer1 = await _readService.Query
                .Top(1)
                .FirstOrDefaultAsync();
            var customer2 = await _readService.Query
                .FirstOrDefaultAsync(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_readService.Query.Cache.Contains<Customer>(
                _readService.Query.LastGeneratedCacheKey));
        }

        [TestMethod]
        public async Task Cached_GetAllAsync()
        {
            var results = await _readService.Query.GetAllAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.IsTrue(_readService.Query.Cache.Contains<Customer>(
                _readService.Query.LastGeneratedCacheKey));
        }
    }
}