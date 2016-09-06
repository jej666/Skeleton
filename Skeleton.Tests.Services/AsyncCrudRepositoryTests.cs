using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class AsyncCrudRepositoryTests : TestBase
    {
        private readonly IAsyncCrudRepository<Customer, int, CustomerDto> _repository;

        public AsyncCrudRepositoryTests()
        {
            _repository = Container.Resolve<IAsyncCrudRepository<Customer, int, CustomerDto>>();

            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public async Task AddAsync()
        {
            var customer = new Customer {Name = "Foo"};
            var successed = await _repository.Store.AddAsync(customer);

            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id > 0);

            var result = await _repository.Query.FirstOrDefaultAsync(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public async Task AddAsync_Multiple()
        {
            var customers = MemorySeeder.SeedCustomers(5);
            var successed = await _repository.Store.AddAsync(customers);

            Assert.IsTrue(successed);
        }

        [TestMethod]
        public async Task DeleteAsync()
        {
            var customer1 = await _repository.Query
                .Top(1)
                .FirstOrDefaultAsync();
            var successed = await _repository.Store.DeleteAsync(customer1);
            Assert.IsTrue(successed);

            var result2 = await _repository.Query.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNull(result2);
        }

        [TestMethod]
        public async Task DeleteAsync_Multiple()
        {
            var customers = await _repository.Query
                .Top(3)
                .FindAsync();
            var successed = await _repository.Store.DeleteAsync(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public async Task UpdateAsync()
        {
            var customer1 = await _repository.Query
                .Top(1)
                .FirstOrDefaultAsync();
            customer1.Name = "CustomerUpdated";
            var successed = await _repository.Store.UpdateAsync(customer1);
            Assert.IsTrue(successed);

            var customer2 = await _repository.Query.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsTrue(customer2.Name.Equals("CustomerUpdated"));
        }

        [TestMethod]
        public async Task UpdateAsync_Multiple()
        {
            var customers = await _repository.Query
                .Top(3)
                .FindAsync();
            var successed = await _repository.Store.UpdateAsync(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public async Task SaveAsync_ShouldAdd()
        {
            var customer = new Customer {Name = "Customer"};
            var successed = await _repository.Store.SaveAsync(customer);
            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id > 0);

            var result = await _repository.Query.FirstOrDefaultAsync(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public async Task SaveAsync_ShouldUpdate()
        {
            var customer = await _repository.Query.Top(1).FirstOrDefaultAsync();
            Assert.IsTrue(customer.Id > 0);

            customer.Name = "CustomerUpdated";
            var successed = await _repository.Store.SaveAsync(customer);
            Assert.IsTrue(successed);

            var result = await _repository.Query.FirstOrDefaultAsync(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Name.Equals("CustomerUpdated"));
        }

        [TestMethod]
        public async Task SaveAsync_Multiple()
        {
            var customers = MemorySeeder.SeedCustomers(5);
            var successed = await _repository.Store.SaveAsync(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void Dispose_Store()
        {
            using (_repository.Store)
            {
            }

            var fieldInfo = typeof(DisposableBase).GetField("_disposed",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(fieldInfo);
            Assert.IsTrue((bool)fieldInfo.GetValue(_repository.Store));
        }
    }
}