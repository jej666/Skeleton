using NUnit.Framework;
using Skeleton.Abstraction.Orm;
using Skeleton.Common;
using Skeleton.Tests.Common;
using System;
using System.Linq;
using System.Reflection;

namespace Skeleton.Tests.Infrastructure
{
    [TestFixture]
    public class CachedEntityReaderTests : OrmTestBase
    {
        private readonly ICachedEntityReader<Customer> _reader;

        public CachedEntityReaderTests()
        {
            _reader = Container.Resolve<ICachedEntityReader<Customer>>();
            _reader.CacheConfigurator =
                config => config.SetAbsoluteExpiration(TimeSpan.FromSeconds(300));
        }

        [Test]
        public void CachedEntityReader_Find_ByExpression()
        {
            var results = _reader
                .Where(c => c.Name.StartsWith("Customer", StringComparison.OrdinalIgnoreCase))
                .Find();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(Customer), results.First());
            Assert.IsTrue(_reader.Cache.Contains(_reader.LastGeneratedCacheKey));
        }

        [Test]
        public void CachedEntityReader_FirstOrDefault_ByExpression()
        {
            var customer1 = _reader
                .Top(1)
                .FirstOrDefault();
            var customer2 = _reader
                .Where(c => c.Name.Equals(customer1.Name))
                .FirstOrDefault();

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOf(typeof(Customer), customer2);
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_reader.Cache.Contains(_reader.LastGeneratedCacheKey));
        }

        [Test]
        public void CachedEntityReader_FirstOrDefault_ById()
        {
            var customer1 = _reader.Top(1).FirstOrDefault();
            var customer2 = _reader.FirstOrDefault(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOf(typeof(Customer), customer2);
            Assert.AreEqual(customer1, customer2);
            Assert.IsTrue(_reader.Cache.Contains(_reader.LastGeneratedCacheKey));
        }

        [Test]
        public void CachedEntityReader_FirstOrDefault_With_Wrong_Id()
        {
            var customer = _reader.FirstOrDefault(100000);

            Assert.IsNull(customer);
        }

        [Test]
        public void CachedEntityReader_GetAll()
        {
            var results = _reader.Find();
            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(Customer), results.First());
            Assert.IsTrue(_reader.Cache.Contains(_reader.LastGeneratedCacheKey));
        }

        [Test]
        public void CachedEntityReader_Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var results = _reader
                    .OrderBy(c => c.CustomerCategoryId)
                    .Query(new Query
                    {
                        PageSize = pageSize,
                        PageNumber = page
                    });

                Assert.IsTrue(results.Count() <= pageSize);
                Assert.IsTrue(_reader.Cache.Contains(
                    _reader.LastGeneratedCacheKey));
            }
        }

        [Test]
        public void CachedEntityReader_Dispose()
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