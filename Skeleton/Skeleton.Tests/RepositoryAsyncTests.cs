//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Skeleton.Common.Reflection;
//using Skeleton.Infrastructure.Data;
//using Skeleton.Tests.Infrastructure;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Skeleton.Tests
//{
//    [TestClass]
//    public class RepositoryAsyncTests : TestBase
//    {
//        private readonly CustomerRepositoryAsync repository;

//        public RepositoryAsyncTests()
//        {
//            var accessorCache = Container.Resolve<ITypeAccessorCache>();
//            var databaseFactory = Container.Resolve<IDatabaseFactory>();
//            repository = new CustomerRepositoryAsync(accessorCache, databaseFactory);
//        }

//        [TestMethod]
//        public async Task AddAsync()
//        {
//            var customer = new Customer() { Name = "Foo" };
//            var successed = await repository.AddAsync(customer);
//            Assert.IsTrue(successed);
//            Assert.IsTrue(customer.Id > 0);
//            var result = await repository.FirstOrDefaultAsync(customer.Id);
//            Assert.IsNotNull(result);
//            Assert.IsInstanceOfType(result, typeof(Customer));
//        }

//        [TestMethod]
//        public async Task AddAsync_Multiple()
//        {
//            var customers = new CustomerSeeder().SeedPosts(5);
//            var successed = await repository.AddAsync(customers);
//            Assert.IsTrue(successed);
//        }

//        [TestMethod]
//        public async Task DeleteAsync()
//        {
//            var sql = repository.QueryBuilder.SelectTop(1).AsSql();
//            var result1 = await repository.FindAsync(sql);
//            var customer = result1.FirstOrDefault();
//            var successed = await repository.DeleteAsync(customer);
//            Assert.IsTrue(successed);

//            var result2 = await repository.FirstOrDefaultAsync(customer.Id);
//            Assert.IsNull(result2);
//        }

//        [TestMethod]
//        public async Task DeleteAsync_Multiple()
//        {
//            var sql = repository.QueryBuilder.SelectTop(3).AsSql();
//            var customers = await repository.FindAsync(sql);
//            var successed = await repository.DeleteAsync(customers);
//            Assert.IsTrue(successed);
//        }

//        [TestMethod]
//        public async Task FindAsync_ByExpression()
//        {
//            var results = await repository.FindAsync(
//                c => c.Name.Equals("Kevin"),
//                c => c.CustomerId);
//            Assert.IsNotNull(results);
//            Assert.IsInstanceOfType(results.First(), typeof(Customer));
//        }

//        [TestMethod]
//        public async Task FirstOrDefaultAsync_ByExpression()
//        {
//            var result1 = await repository.FirstOrDefaultAsync(c => c.Name.Equals("Foo"));
//            Assert.IsNotNull(result1);
//            Assert.IsInstanceOfType(result1, typeof(Customer));

//            var result2 = await repository.FirstOrDefaultAsync(c => c.Name.Equals("Foo"));
//            Assert.AreEqual(result1, result2);
//        }

//        [TestMethod]
//        public async Task FirstOrDefaultAsync_ById()
//        {
//            var sql = repository.QueryBuilder.SelectTop(1).AsSql();
//            var result = await repository.FindAsync(sql);
//            var customer1 = result.FirstOrDefault();
//            var customer2 = await repository.FirstOrDefaultAsync(customer1.Id);
//            Assert.IsNotNull(customer2);
//            Assert.IsInstanceOfType(customer2, typeof(Customer));
//            Assert.AreEqual(customer1, customer2);
//        }

//        [TestMethod]
//        public async Task GetAllAsync()
//        {
//            var results = await repository.GetAllAsync();
//            Assert.IsNotNull(results);
//            Assert.IsInstanceOfType(results.First(), typeof(Customer));
//        }

//        [TestMethod]
//        public async Task UpdateAsync()
//        {
//            var sql = repository.QueryBuilder.SelectTop(1).AsSql();
//            var result1 = await repository.FindAsync(sql);
//            var customer = result1.FirstOrDefault();
//            customer.Name = "UpdatedName";
//            var successed = await repository.UpdateAsync(customer);
//            Assert.IsTrue(successed);

//            var result2 = await repository.FirstOrDefaultAsync(customer.Id);
//            Assert.IsNotNull(result2);
//            Assert.IsTrue(result2.Name.Equals("UpdatedName"));
//        }

//        [TestMethod]
//        public async Task UpdateAsync_Multiple()
//        {
//            var sql = repository.QueryBuilder.SelectTop(3).AsSql();
//            var customers = await repository.FindAsync(sql);
//            foreach (var cust in customers)
//                cust.Name = "Updated" + cust.Id;

//            var successed = await repository.UpdateAsync(customers);
//            Assert.IsTrue(successed);
//        }
//    }
//}

