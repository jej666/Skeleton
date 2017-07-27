using NUnit.Framework;
using Skeleton.Abstraction.Orm;
using Skeleton.Core;
using Skeleton.Tests.Common;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Skeleton.Tests.Infrastructure
{
    [TestFixture]
    public class AsyncEntityReaderTests : OrmTestBase
    {
        private readonly IAsyncEntityReader<Customer> _reader;

        public AsyncEntityReaderTests()
        {
            _reader = Resolver.Resolve<IAsyncEntityReader<Customer>>();
        }

        private async Task<Customer> GetAsyncFirstCustomer()
        {
            return await _reader
                .Top(1)
                .FirstOrDefaultAsync();
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_EqualsMethod()
        {
            var customer = await GetAsyncFirstCustomer();
            var results = await _reader
                .Where(c => c.Name.Equals(customer.Name))
                .OrderBy(c => c.CustomerId)
                .FindAsync();

            Assert.IsNotNull(results);
            var firstResult = results.First();
            Assert.IsInstanceOf(typeof(Customer), firstResult);
        }

        [Test]
        public async Task AsyncEntityReader_FirstOrDefaultAsync_StartWith()
        {
            var result = await _reader
                .Where(c => c.Name.StartsWith("Customer", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(Customer), result);
        }

        [Test]
        public async Task AsyncEntityReader_FirstOrDefaultAsync_ById()
        {
            var customer1 = await GetAsyncFirstCustomer();
            var customer2 = await _reader
                .FirstOrDefaultAsync(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOf(typeof(Customer), customer2);
            Assert.AreEqual(customer1, customer2);
        }

        [Test]
        public async Task AsyncEntityReader_GetAllAsync()
        {
            var results = await _reader.FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(Customer), results.First());
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_Selected_Columns()
        {
            var customer = await GetAsyncFirstCustomer();
            var results = await _reader
                .Where(c => c.CustomerId == customer.CustomerId)
                .Select(c => c.CustomerId)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(Customer), results.First());
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_Top()
        {
            var customers = await _reader.Top(5).FindAsync();

            Assert.IsNotNull(customers);
            Assert.IsInstanceOf(typeof(Customer), customers.First());
            Assert.IsTrue(customers.Count() == 5);
        }

        [Test]
        public async Task AsyncEntityReader_CountAsync()
        {
            var count = await _reader
                .CountAsync(c => c.CustomerId);

            Assert.IsNotNull(count);
            Assert.IsTrue(count.FirstOrDefault().CountCustomerId > 0);
        }

        [Test]
        public async Task AsyncEntityReader_CountAllAsync()
        {
            var count = await _reader.CountAsync();

            Assert.IsNotNull(count);
            Assert.IsTrue(count > 0);
        }

        [Test]
        public async Task AsyncEntityReader_MinAsync()
        {
            var minCustomer = await _reader
                .OrderBy(c => c.CustomerId)
                .Top(1)
                .FirstOrDefaultAsync();
            var min = await _reader
                .MinAsync(c => c.CustomerId);

            Assert.IsNotNull(min);
            Assert.IsTrue(min.FirstOrDefault().MinCustomerId == minCustomer.CustomerId);
        }

        [Test]
        public async Task AsyncEntityReader_MaxAsync()
        {
            var maxCustomer = await _reader
                .OrderByDescending(c => c.CustomerId)
                .Top(1)
                .FirstOrDefaultAsync();
            var max = await _reader
                .MaxAsync(c => c.CustomerId);

            Assert.IsNotNull(max);
            Assert.IsTrue(max.FirstOrDefault().MaxCustomerId == maxCustomer.CustomerId);
        }

        [Test]
        public async Task AsyncEntityReader_AverageAsync()
        {
            var avg = await _reader
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .Select(c => c.CustomerId)
                .AverageAsync(c => c.CustomerCategoryId);

            var result = avg.FirstOrDefault();

            Assert.IsNotNull(avg);
            Assert.IsTrue(result.AvgCustomerCategoryId > 0);
        }

        [Test]
        public async Task AsyncEntityReader_SumAsync()
        {
            var sum = await _reader
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .SumAsync(c => c.CustomerCategoryId);

            var result = sum.FirstOrDefault();

            Assert.IsNotNull(sum);
            Assert.IsTrue(result.SumCustomerCategoryId > 0);
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_LeftJoin()
        {
            var results = await _reader.LeftJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_RightJoin()
        {
            var results = await _reader.RightJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_RightJoin_Distinct()
        {
            var results = await _reader.RightJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Distinct(customer => customer.CustomerId)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_InnerJoin()
        {
            var results = await _reader.InnerJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_Where_Is_In()
        {
            var customerIds = new object[] { 5, 15, 25 };
            var results = await _reader
                .WhereIsIn(c => c.CustomerId, customerIds)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => customerIds.Contains(c.CustomerId)));
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_Where_Not_In()
        {
            var customerIds = new object[] { 5, 15, 25 };
            var results = await _reader
                .WhereNotIn(c => c.CustomerId, customerIds)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => !customerIds.Contains(c.CustomerId)));
        }

        [Test]
        public async Task AsyncEntityReader_PageAsync()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var results = await _reader
                    .OrderBy(c => c.CustomerCategoryId)
                    .QueryAsync(new Query
                    {
                        PageSize = pageSize,
                        PageNumber = page
                    });

                Assert.IsTrue(results.Count() <= pageSize);
            }
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_Where_Is_Null()
        {
            var results = await _reader
                .Where(c => c.Name == null)
                .FindAsync();

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_Where_Is_Not_Null()
        {
            var results = await _reader
                .Where(c => c.Name != null)
                .FindAsync();

            Assert.IsNotNull(results);
        }

        [Test]
        public async Task AsyncEntityReader_FindAsync_Where_Complex()
        {
            var results = await _reader
                .Where(c => (c.CustomerId >= 1)
                            && c.Name.Contains("Customer"))
                .FindAsync();

            Assert.IsNotNull(results);
        }

        [Test]
        public void AsyncEntityReader_Dispose()
        {
            using (_reader)
            {
            }

            var fieldInfo = typeof(DisposableBase).GetField("_disposed",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(fieldInfo);
            Assert.IsTrue((bool)fieldInfo.GetValue(_reader));
        }
    }
}