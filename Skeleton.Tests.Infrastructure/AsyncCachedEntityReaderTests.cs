using NUnit.Framework;
using Skeleton.Abstraction.Orm;
using Skeleton.Common;
using Skeleton.Tests.Common;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Skeleton.Tests.Infrastructure
{
    [TestFixture]
    public class AsyncCachedEntityReaderTests : OrmTestBase
    {
        private readonly IAsyncCachedEntityReader<Customer> _reader;

        public AsyncCachedEntityReaderTests()
        {
            _reader = Container.Resolve<IAsyncCachedEntityReader<Customer>>();
        }

        [Test]
        public async Task AsyncCachedEntityReader_FindAsync_ByExpression()
        {
            var results = await _reader
                .Where(c => c.Name.StartsWith("Customer", StringComparison.InvariantCultureIgnoreCase))
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(Customer), results.First());
            Assert.IsTrue(await _reader.Cache.ContainsAsync(
                _reader.LastGeneratedCacheKey));
        }

        [Test]
        public async Task AsyncCachedEntityReader_FirstOrDefaultAsync_ByExpression()
        {
            var customer1 = await _reader
                .Top(1)
                .FirstOrDefaultAsync();
            var customer2 = await _reader
                .Where(c => c.Name.Equals(customer1.Name))
                .FirstOrDefaultAsync();

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOf(typeof(Customer), customer2);
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(await _reader.Cache.ContainsAsync(
                _reader.LastGeneratedCacheKey));
        }

        [Test]
        public async Task AsyncCachedEntityReader_FirstOrDefaultAsync_ById()
        {
            var customer1 = await _reader
                .Top(1)
                .FirstOrDefaultAsync();
            var customer2 = await _reader
                .FirstOrDefaultAsync(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOf(typeof(Customer), customer2);
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(await _reader.Cache.ContainsAsync(
                _reader.LastGeneratedCacheKey));
        }

        [Test]
        public async Task AsyncCachedEntityReader_GetAllAsync()
        {
            var results = await _reader.FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(Customer), results.First());
            Assert.IsTrue(await _reader.Cache.ContainsAsync(
                _reader.LastGeneratedCacheKey));
        }

        [Test]
        public async Task AsyncCachedEntityReader_PageAsync()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var results = await _reader
                    .OrderBy(c => c.CustomerCategoryId)
                    .QueryAsync(new Query
                    {
                        PageSize = pageSize,
                        PageNumber = page
                    });

                Assert.IsTrue(results.Count() <= pageSize);
                Assert.IsTrue(await _reader.Cache.ContainsAsync(
                    _reader.LastGeneratedCacheKey));
            }
        }

        [Test]
        public void AsyncCachedEntityReader_Dispose()
        {
            using (_reader)
            {
            }

            var fieldInfo = typeof(DisposableBase).GetField("_disposed",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(fieldInfo);
            Assert.IsTrue((bool)fieldInfo.GetValue(_reader));
        }
    }
}