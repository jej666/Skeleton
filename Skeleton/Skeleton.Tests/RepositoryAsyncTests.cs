using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Common.Reflection;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class RepositoryAsyncTests : TestBase
    {
        private readonly IRepositoryAsync<Customer, int> _repository;

        public RepositoryAsyncTests()
        {
            _repository = Container.Resolve<IRepositoryAsync<Customer,int>>();

            Seeder.SeedCustomers();
        }

        [TestMethod]
        public async Task AddAsync()
        {
            var customer = new Customer {Name = "Foo"};
            var successed = await _repository.AddAsync(customer);
            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id > 0);
            var result = await _repository.FirstOrDefaultAsync(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public async Task AddAsync_Multiple()
        {
            var customers = Seeder.SeedCustomers(5);
            var successed = await _repository.AddAsync(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public async Task DeleteAsync()
        {
            var customer1 = await _repository
                .SelectTop(1)
                .FirstOrDefaultAsync();
            var successed = await _repository.DeleteAsync(customer1);
            Assert.IsTrue(successed);

            var result2 = await _repository.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNull(result2);
        }

        [TestMethod]
        public async Task DeleteAsync_Multiple()
        {
            var customers = await _repository
                .SelectTop(3)
                .FindAsync();
            var successed = await _repository.DeleteAsync(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public async Task UpdateAsync()
        {
            var customer1 = await _repository
                .SelectTop(1)
                .FirstOrDefaultAsync();
            customer1.Name = "UpdatedName";
            var successed = await _repository.UpdateAsync(customer1);
            Assert.IsTrue(successed);

            var customer2 = await _repository.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsTrue(customer2.Name.Equals("UpdatedName"));
        }

        [TestMethod]
        public async Task UpdateAsync_Multiple()
        {
            var customers = await _repository
                .SelectTop(3)
                .FindAsync();
            var successed = await _repository.UpdateAsync(customers);
            Assert.IsTrue(successed);
        }
    }
}