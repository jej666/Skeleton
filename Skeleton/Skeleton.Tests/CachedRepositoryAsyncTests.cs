//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Skeleton.Common;
//using Skeleton.Common.Reflection;
//using Skeleton.Infrastructure.Data;
//using Skeleton.Tests.Infrastructure;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Skeleton.Tests
//{
//    [TestClass]
//    public class CachedRepositoryAsyncTests : TestBase
//    {
//        private readonly CachedCustomerRepositoryAsync repository;

//        public CachedRepositoryAsyncTests()
//        {
//            var accessorCache = Container.Resolve<ITypeAccessorCache>();
//            var database = Container.Resolve<IDatabaseAsync>();
//            var cacheProvider = Container.Resolve<ICacheProvider>();
//            repository = new CachedCustomerRepositoryAsync(accessorCache, cacheProvider, database);
//        }

//        [TestMethod]
//        public async Task FindAsync_ByExpression()
//        {
//            var results = await repository.FindAsync(
//                c => c.Name.Equals("Kevin"),
//                c => c.CustomerId);

//            Assert.IsNotNull(results);
//            Assert.IsInstanceOfType(results.First(), typeof(Customer));

//            var parameters = new Dictionary<string, object>() { { "P1", "Kevin" } };
//            Assert.IsTrue(repository.Cache.Contains<Customer>(
//                CustomerCacheKey.ForFind(repository.QueryBuilder.AsSql())));
//        }

//        [TestMethod]
//        public async Task FirstOrDefaultAsync_ByExpression()
//        {
//            var customer1 = await repository.FirstOrDefaultAsync(
//                c => c.Name.Equals("Foo"));
//            Assert.IsNotNull(customer1);
//            Assert.IsInstanceOfType(customer1, typeof(Customer));

//            var customer2 = await repository.FirstOrDefaultAsync(
//                c => c.Name.Equals("Foo"));

//            Assert.AreEqual(customer1, customer2);
//            var parameters = new Dictionary<string, object>() { { "P1", "Foo" } };
//            Assert.IsTrue(repository.Cache.Contains<Customer>(
//                CustomerCacheKey.ForFirstOrDefault(parameters)));
//        }

//        [TestMethod]
//        public async Task FirstOrDefaultAsync_ById()
//        {
//            var sql = repository.QueryBuilder.SelectTop(1).AsSql();
//            var result = await repository.FindAsync(sql);
//            var customer1= result.FirstOrDefault();
//            var customer2 = await repository.FirstOrDefaultAsync(customer1.Id);

//            Assert.IsNotNull(customer2);
//            Assert.IsInstanceOfType(customer2, typeof(Customer));
//            Assert.AreEqual(customer1, customer2);
//            Assert.IsTrue(repository.Cache.Contains<Customer>(
//                CustomerCacheKey.ForFirstOrDefault(customer1.Id)));
//        }

//        [TestMethod]
//        public async Task GetAllAsync()
//        {
//            var results = await repository.GetAllAsync();
//            Assert.IsNotNull(results);
//            Assert.IsInstanceOfType(results.First(), typeof(Customer));
//            Assert.IsTrue(repository.Cache.Contains<Customer>(
//                CustomerCacheKey.ForGetAll()));
//        }
//    }
//}

