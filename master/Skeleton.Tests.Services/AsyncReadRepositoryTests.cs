using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Tests.Common;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.Tests.Infrastructure
{
    [TestClass]
    public class AsyncReadRepositoryTests : RepositoryTestBase
    {
        private readonly IAsyncCrudRepository<Customer, CustomerDto> _repository;

        public AsyncReadRepositoryTests()
        {
            _repository = Container.Resolve<IAsyncCrudRepository<Customer, CustomerDto>>();

            SqlDbSeeder.SeedCustomers();
        }

        private async Task<Customer> GetAsyncFirstCustomer()
        {
            return await _repository.Query
                .Top(1)
                .FirstOrDefaultAsync();
        }

        [TestMethod]
        public async Task FindAsync_EqualsMethod()
        {
            var customer = await GetAsyncFirstCustomer();
            var results = await _repository.Query
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
            var result = await _repository.Query
                .Where(c => c.Name.StartsWith("Customer", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ById()
        {
            var customer1 = await GetAsyncFirstCustomer();
            var customer2 = await _repository.Query
                .FirstOrDefaultAsync(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [TestMethod]
        public async Task GetAllAsync()
        {
            var results = await _repository.Query.FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        [TestMethod]
        public async Task FindAsync_Selected_Columns()
        {
            var customer = await GetAsyncFirstCustomer();
            var results = await _repository.Query
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
            var customers = await _repository.Query.Top(5).FindAsync();

            Assert.IsNotNull(customers);
            Assert.IsInstanceOfType(customers.First(), typeof(Customer));
            Assert.IsTrue(customers.Count() == 5);
        }

        [TestMethod]
        public async Task CountAsync()
        {
            var count = await _repository.Query
                .CountAsync(c => c.CustomerId);

            Assert.IsNotNull(count);
            Assert.IsTrue(count.FirstOrDefault().CountCustomerId > 0);
        }

        [TestMethod]
        public async Task CountAllAsync()
        {
            var count = await _repository.Query.CountAsync();

            Assert.IsNotNull(count);
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public async Task MinAsync()
        {
            var minCustomer = await _repository.Query
                .OrderBy(c => c.CustomerId)
                .Top(1)
                .FirstOrDefaultAsync();
            var min = await _repository.Query
                .MinAsync(c => c.CustomerId);

            Assert.IsNotNull(min);
            Assert.IsTrue(min.FirstOrDefault().MinCustomerId == minCustomer.CustomerId);
        }

        [TestMethod]
        public async Task MaxAsync()
        {
            var maxCustomer = await _repository.Query
                .OrderByDescending(c => c.CustomerId)
                .Top(1)
                .FirstOrDefaultAsync();
            var max = await _repository.Query
                .MaxAsync(c => c.CustomerId);

            Assert.IsNotNull(max);
            Assert.IsTrue(max.FirstOrDefault().MaxCustomerId == maxCustomer.CustomerId);
        }

        [TestMethod]
        public async Task AverageAsync()
        {
            var avg = await _repository.Query
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .Select(c => c.CustomerId)
                .AverageAsync(c => c.CustomerCategoryId);

            var result = avg.FirstOrDefault();

            Assert.IsNotNull(avg);
            Assert.IsTrue(result.AvgCustomerCategoryId > 0);
        }

        [TestMethod]
        public async Task SumAsync()
        {
            var sum = await _repository.Query
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .SumAsync(c => c.CustomerCategoryId);

            var result = sum.FirstOrDefault();

            Assert.IsNotNull(sum);
            Assert.IsTrue(result.SumCustomerCategoryId > 0);
        }


        [TestMethod]
        public async Task FindAsync_LeftJoin()
        {
            var results = await _repository.Query.LeftJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public async Task FindAsync_RightJoin()
        {
            var results = await _repository.Query.RightJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public async Task FindAsync_RightJoin_Distinct()
        {
            var results = await _repository.Query.RightJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Distinct(customer => customer.CustomerId)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public async Task FindAsync_InnerJoin()
        {
            var results = await _repository.Query.InnerJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public async Task FindAsync_Where_Is_In()
        {
            var customerIds = new object[] {5, 15, 25};
            var results = await _repository.Query
                .WhereIsIn(c => c.CustomerId, customerIds)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => customerIds.Contains(c.CustomerId)));
        }

        [TestMethod]
        public async Task FindAsync_Where_Not_In()
        {
            var customerIds = new object[] {5, 15, 25};
            var results = await _repository.Query
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
                var results = await _repository.Query
                    .OrderBy(c => c.CustomerCategoryId)
                    .PageAsync(pageSize, page);

                Assert.IsTrue(results.Count() <= pageSize);
            }
        }

        [TestMethod]
        public async Task FindAsync_Where_Is_Null()
        {
            var results = await _repository.Query
                .Where(c => c.Name == null)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public async Task FindAsync_Where_Is_Not_Null()
        {
            var results = await _repository.Query
                .Where(c => c.Name != null)
                .FindAsync();

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public async Task FindAsync_Where_Complex()
        {
            var results = await _repository.Query
                .Where(c => (c.CustomerId >= 1)
                            && c.Name.Contains("Customer"))
                .FindAsync();

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void Dispose_Query()
        {
            using (_repository.Query)
            {
            }

            var fieldInfo = typeof(DisposableBase).GetField("_disposed",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(fieldInfo);
            Assert.IsTrue((bool) fieldInfo.GetValue(_repository.Query));
        }
    }
}