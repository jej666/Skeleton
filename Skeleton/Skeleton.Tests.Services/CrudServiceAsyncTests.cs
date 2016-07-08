using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Core.Service;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class CrudServiceAsyncTests : TestBase
    {
        private readonly ICrudServiceAsync<Customer, int> _service;

        public CrudServiceAsyncTests()
        {
            _service = Container.Resolve<ICrudServiceAsync<Customer, int>>();

            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public async Task AddAsync()
        {
            var customer = new Customer {Name = "Foo"};
            var successed = await _service.Repository.AddAsync(customer);
            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id > 0);
            var result = await _service.Repository.FirstOrDefaultAsync(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public async Task AddAsync_Multiple()
        {
            var customers = MemorySeeder.SeedCustomers(5);
            var successed = await _service.Repository.AddAsync(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public async Task DeleteAsync()
        {
            var customer1 = await _service.Repository
                .SelectTop(1)
                .FirstOrDefaultAsync();
            var successed = await _service.Repository.DeleteAsync(customer1);
            Assert.IsTrue(successed);

            var result2 = await _service.Repository.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNull(result2);
        }

        [TestMethod]
        public async Task DeleteAsync_Multiple()
        {
            var customers = await _service.Repository
                .SelectTop(3)
                .FindAsync();
            var successed = await _service.Repository.DeleteAsync(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public async Task UpdateAsync()
        {
            var customer1 = await _service.Repository
                .SelectTop(1)
                .FirstOrDefaultAsync();
            customer1.Name = "UpdatedName";
            var successed = await _service.Repository.UpdateAsync(customer1);
            Assert.IsTrue(successed);

            var customer2 = await _service.Repository.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsTrue(customer2.Name.Equals("UpdatedName"));
        }

        [TestMethod]
        public async Task UpdateAsync_Multiple()
        {
            var customers = await _service.Repository
                .SelectTop(3)
                .FindAsync();
            var successed = await _service.Repository.UpdateAsync(customers);
            Assert.IsTrue(successed);
        }
    }
}