using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Core.Service;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class ReadServiceAsyncTests : TestBase
    {
        private readonly ICrudServiceAsync<Customer, int> _service;

        public ReadServiceAsyncTests()
        {
            _service = Container.Resolve<ICrudServiceAsync<Customer, int>>();

            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public async Task FindAsync_ByExpression()
        {
            var results = await _service.Repository
                .Where(c => c.Name.StartsWith("Customer"))
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ByExpression()
        {
            var customer = await _service.Repository
                .SelectTop(1)
                .FirstOrDefaultAsync();
            var result = await _service.Repository
                .Where(c => c.Name.Equals(customer.Name))
                .FirstOrDefaultAsync();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ById()
        {
            var customer1 = await _service.Repository
                .SelectTop(1)
                .FirstOrDefaultAsync();
            var customer2 = await _service.Repository
                .FirstOrDefaultAsync(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
        }

        [TestMethod]
        public async Task GetAllAsync()
        {
            var results = await _service.Repository.GetAllAsync();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(r => r.GetType() == typeof(Customer)));
        }
    }
}