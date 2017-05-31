using NUnit.Framework;
using Skeleton.Tests.Common;
using System;
using System.Threading.Tasks;

namespace Skeleton.Tests.Core
{
    [TestFixture]
    public class CacheTests : CoreTestsBase
    {
        private static readonly string CustomerKey = typeof(Customer).Name;
        private static readonly TimeSpan Expiration = TimeSpan.FromSeconds(30);

        private void GetOrAdd()
        {
            CacheProvider.GetOrAdd(
                  CustomerKey,
                  () => new Customer { CustomerId = 1 },
                  configurator => configurator.SetAbsoluteExpiration(Expiration));
        }

        private async Task GetOrAddAsync()
        {
            await AsyncCacheProvider.GetOrAddAsync(
                    CustomerKey,
                    () => Task.Factory.StartNew(() => new Customer { CustomerId = 1 }),
                    configurator => configurator.SetSlidingExpiration(Expiration));
        }

        [Test]
        public void Cache_Can_Store()
        {
            GetOrAdd();

            Assert.IsTrue(CacheProvider.Contains(CustomerKey));
        }

        [Test]
        public async Task Cache_Can_StoreAsync()
        {
            await GetOrAddAsync();

            Assert.IsTrue(AsyncCacheProvider.Contains(CustomerKey));
        }

        [Test]
        public void Cache_Can_Remove()
        {
            GetOrAdd();
            Assert.IsTrue(CacheProvider.Contains(CustomerKey));

            CacheProvider.Remove(CustomerKey);
            Assert.IsFalse(CacheProvider.Contains(CustomerKey));
        }

        [Test]
        public async Task Cache_Can_RemoveAsync()
        {
            await GetOrAddAsync();
            Assert.IsTrue(AsyncCacheProvider.Contains(CustomerKey));

            AsyncCacheProvider.Remove(CustomerKey);
            Assert.IsFalse(AsyncCacheProvider.Contains(CustomerKey));
        }
    }
}