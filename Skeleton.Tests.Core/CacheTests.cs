using NUnit.Framework;
using Skeleton.Abstraction;
using Skeleton.Tests.Common;
using System;

namespace Skeleton.Tests.Core
{
    [TestFixture]
    public class CacheTests : CoreTestsBase
    {
        ICacheProvider CacheProvider = Container.Resolve<ICacheProvider>();
        static string CustomerKey = typeof(Customer).Name;
        static TimeSpan AbsoluteExpiration = TimeSpan.FromSeconds(30);

        private void GetOrAdd()
        {
            CacheProvider.GetOrAdd(
                  CustomerKey,
                  () => new Customer { CustomerId = 1 },
                  configurator => configurator.SetAbsoluteExpiration(AbsoluteExpiration));
        }

        [Test]
        public void Cache_Can_Store()
        {
            GetOrAdd();

            Assert.IsTrue(CacheProvider.Contains(CustomerKey));
        }       

        [Test]
        public void Cache_Can_Remove()
        {
            GetOrAdd();

            Assert.IsTrue(CacheProvider.Contains(CustomerKey));
            CacheProvider.Remove(CustomerKey);
            Assert.IsFalse(CacheProvider.Contains(CustomerKey));
        }
    }
}
