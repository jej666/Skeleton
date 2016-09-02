using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Core.Repository;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class AsyncReadRepositoryTests : TestBase
    {
        private readonly IAsyncCrudRepository<Customer, int, CustomerDto> _service;

        public AsyncReadRepositoryTests()
        {
            _service = Container.Resolve<IAsyncCrudRepository<Customer, int, CustomerDto>>();

            SqlDbSeeder.SeedCustomers();
        }

        private async Task<Customer> GetAsyncFirstCustomer()
        {
            return await _service.Query
                .Top(1)
                .FirstOrDefaultAsync();
        }

        [TestMethod]
        public async Task FindAsync_EqualsMethod()
        {
            var customer = await GetAsyncFirstCustomer();
            var results = await _service.Query
                .Where(c => c.Name.Equals(customer.Name))
                .OrderBy(c => c.CustomerId)
                .FindAsync();

            Assert.IsNotNull(results);
            var firstResult = results.First();
            Assert.IsInstanceOfType(firstResult, typeof(Customer));
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_StartWith()
        {
            var result = await _service.Query
                .Where(c => c.Name.StartsWith("Customer"))
                .FirstOrDefaultAsync();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ById()
        {
            var customer1 = await GetAsyncFirstCustomer();
            var customer2 = await _service.Query
                .FirstOrDefaultAsync(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
        }

        [TestMethod]
        public async Task GetAllAsync()
        {
            var results = await _service.Query.GetAllAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        [TestMethod]
        public async Task FindAsync_SelectedColumns()
        {
            var customer = await GetAsyncFirstCustomer();
            var results = await _service.Query
                .Where(c => c.CustomerId == customer.CustomerId)
                .Select(c => c.CustomerId)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public async Task FindAsync_Top()
        {
            var customers = await _service.Query.Top(5).FindAsync();

            Assert.IsNotNull(customers);
            Assert.IsInstanceOfType(customers.First(), typeof(Customer));
            Assert.IsTrue(customers.Count() == 5);
        }

        [TestMethod]
        public async Task CountAsync()
        {
            var count = await _service.Query
                .CountAsync(c => c.CustomerId);

            Assert.IsNotNull(count);
            Assert.IsTrue(count.FirstOrDefault().Count > 0);
        }

        [TestMethod]
        public async Task CountAllAsync()
        {
            var count = await _service.Query.CountAsync();

            Assert.IsNotNull(count);
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public async Task MinAsync()
        {
            var minCustomer = await _service.Query
                .OrderBy(c => c.CustomerId)
                .Top(1)
                .FirstOrDefaultAsync();
            var min = await _service.Query
                .MinAsync(c => c.CustomerId);

            Assert.IsNotNull(min);
            Assert.IsTrue(min.FirstOrDefault().Min == minCustomer.CustomerId);
        }

        [TestMethod]
        public async Task MaxAsync()
        {
            var maxCustomer = await _service.Query
                .OrderByDescending(c => c.CustomerId)
                .Top(1)
                .FirstOrDefaultAsync();
            var max = await _service.Query
                .MaxAsync(c => c.CustomerId);

            Assert.IsNotNull(max);
            Assert.IsTrue(max.FirstOrDefault().Max == maxCustomer.CustomerId);
        }

        [TestMethod]
        public async Task AverageAsync()
        {
            var avg = await _service.Query
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .Select(c => c.CustomerId)
                .AverageAsync(c => c.CustomerCategoryId);

            var result = avg.FirstOrDefault();

            Assert.IsNotNull(avg);
            Assert.IsTrue(result.Avg > 0);
        }

        [TestMethod]
        public async Task SumAsync()
        {
            var sum = await _service.Query
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .SumAsync(c => c.CustomerCategoryId);

            var result = sum.FirstOrDefault();

            Assert.IsNotNull(sum);
            Assert.IsTrue(result.Sum > 0);
        }


        [TestMethod]
        public async Task FindAsync_LeftJoin()
        {
            var results = await _service.Query.LeftJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public async Task FindAsync_WhereIsIn()
        {
            var customerIds = new object[] {5, 15, 25};
            var results = await _service.Query
                .WhereIsIn(c => c.CustomerId, customerIds)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => customerIds.Contains(c.CustomerId)));
        }

        [TestMethod]
        public async Task FindAsync_WhereNotIn()
        {
            var customerIds = new object[] {5, 15, 25};
            var results = await _service.Query
                .WhereNotIn(c => c.CustomerId, customerIds)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => !customerIds.Contains(c.CustomerId)));
        }

        [TestMethod]
        public async Task PageAsync()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var results = await _service.Query
                    .OrderBy(c => c.CustomerCategoryId)
                    .PageAsync(pageSize, page);

                Assert.AreEqual(pageSize, results.Count());
            }
        }

        [TestMethod]
        public async Task FindAsync_WhereIsNull()
        {
            var results = await _service.Query
                .Where(c => c.Name == null)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public async Task FindAsync_WhereIsNotNull()
        {
            var results = await _service.Query
                .Where(c => c.Name != null)
                .FindAsync();

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public async Task FindAsync_ComplexWhere()
        {
            var results = await _service.Query
                .Where(c => (c.CustomerId >= 1)
                            && c.Name.Contains("Customer"))
                .FindAsync();

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public async Task FindAsync_EqualsOperator()
        {
            var customer = await GetAsyncFirstCustomer();
            var results = await _service.Query
                .Where(c => c.CustomerId == customer.Id)
                .FindAsync();

            Assert.IsNotNull(results);
        }
    }
}