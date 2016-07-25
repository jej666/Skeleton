using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Core.Service;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class AsyncCrudServiceTests : TestBase
    {
        private readonly IAsyncCrudService<Customer, int, CustomerDto> _service;

        public AsyncCrudServiceTests()
        {
            _service = Container.Resolve<IAsyncCrudService<Customer, int, CustomerDto>>();

            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public async Task AddAsync()
        {
            var customer = new Customer {Name = "Foo"};
            var successed = await _service.Store.AddAsync(customer);

            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id > 0);

            var result = await _service.Query.FirstOrDefaultAsync(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public async Task AddAsync_Multiple()
        {
            var customers = MemorySeeder.SeedCustomers(5);
            var successed = await _service.Store.AddAsync(customers);

            Assert.IsTrue(successed);
        }

        [TestMethod]
        public async Task DeleteAsync()
        {
            var customer1 = await _service.Query
                .Top(1)
                .FirstOrDefaultAsync();
            var successed = await _service.Store.DeleteAsync(customer1);
            Assert.IsTrue(successed);

            var result2 = await _service.Query.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNull(result2);
        }

        [TestMethod]
        public async Task DeleteAsync_Multiple()
        {
            var customers = await _service.Query
                .Top(3)
                .FindAsync();
            var successed = await _service.Store.DeleteAsync(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public async Task UpdateAsync()
        {
            var customer1 = await _service.Query
                .Top(1)
                .FirstOrDefaultAsync();
            customer1.Name = "CustomerUpdated";
            var successed = await _service.Store.UpdateAsync(customer1);
            Assert.IsTrue(successed);

            var customer2 = await _service.Query.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsTrue(customer2.Name.Equals("CustomerUpdated"));
        }

        [TestMethod]
        public async Task UpdateAsync_Multiple()
        {
            var customers = await _service.Query
                .Top(3)
                .FindAsync();
            var successed = await _service.Store.UpdateAsync(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public async Task SaveAsync_ShouldAdd()
        {
            var customer = new Customer { Name = "Customer" };
            var successed = await _service.Store.SaveAsync(customer);
            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id > 0);

            var result = await _service.Query.FirstOrDefaultAsync(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public async Task SaveAsync_ShouldUpdate()
        {
            var customer =await _service.Query.Top(1).FirstOrDefaultAsync();
            Assert.IsTrue(customer.Id > 0);

            customer.Name = "CustomerUpdated";
            var successed =await _service.Store.SaveAsync(customer);
            Assert.IsTrue(successed);

            var result = await _service.Query.FirstOrDefaultAsync(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Name.Equals("CustomerUpdated"));
        }

        [TestMethod]
        public async Task SaveAsync_Multiple()
        {
            var customers = MemorySeeder.SeedCustomers(5);
            var successed =await  _service.Store.SaveAsync(customers);
            Assert.IsTrue(successed);
        }
    }
}