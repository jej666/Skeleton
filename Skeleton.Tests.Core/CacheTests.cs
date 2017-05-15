using NUnit.Framework;
using Skeleton.Abstraction;
using Skeleton.Tests.Common;
using System;
using System.Threading.Tasks;

namespace Skeleton.Tests.Core
{
    [TestFixture]
    public class CacheTests : CoreTestsBase
    {
        ICacheProvider _cacheProvider = Container.Resolve<ICacheProvider>();
        IAsyncCacheProvider _asyncCacheProvider = Container.Resolve<IAsyncCacheProvider>();

        static string CustomerKey = typeof(Customer).Name;
        static TimeSpan Expiration = TimeSpan.FromSeconds(30);

        private void GetOrAdd()
        {
            _cacheProvider.GetOrAdd(
                  CustomerKey,
                  () => new Customer { CustomerId = 1 },
                  configurator => configurator.SetAbsoluteExpiration(Expiration));
        }

        private async Task GetOrAddAsync()
        {
            await _asyncCacheProvider.GetOrAddAsync(
                    CustomerKey,
                    () => Task.Factory.StartNew(() => new Customer { CustomerId = 1 }),
                    configurator => configurator.SetSlidingExpiration(Expiration));
        }

        [Test]
        public void Cache_Can_Store()
        {
            GetOrAdd();

            Assert.IsTrue(_cacheProvider.Contains(CustomerKey));
        }

        [Test]
        public async Task Cache_Can_StoreAsync()
        {
            await GetOrAddAsync();

            Assert.IsTrue(await _asyncCacheProvider.ContainsAsync(CustomerKey));
        }

        [Test]
        public void Cache_Can_Remove()
        {
            GetOrAdd();
            Assert.IsTrue(_cacheProvider.Contains(CustomerKey));

            _cacheProvider.Remove(CustomerKey);
            Assert.IsFalse(_cacheProvider.Contains(CustomerKey));
        }

        [Test]
        public async Task  Cache_Can_RemoveAsync()
        {
            await GetOrAddAsync();
            Assert.IsTrue(await _asyncCacheProvider.ContainsAsync(CustomerKey));

            await _asyncCacheProvider.RemoveAsync(CustomerKey);
            Assert.IsFalse(await _asyncCacheProvider.ContainsAsync(CustomerKey));
        }
    }
}