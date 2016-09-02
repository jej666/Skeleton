using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Core.Repository;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class ReadServiceTests : TestBase
    {
        private readonly ICrudService<Customer, int, CustomerDto> _service;

        public ReadServiceTests()
        {
            _service = Container.Resolve<ICrudService<Customer, int, CustomerDto>>();

            SqlDbSeeder.SeedCustomers();
        }

        private Customer GetFirstCustomer()
        {
            return _service.Query
                .Top(1)
                .FirstOrDefault();
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_EqualsMethod()
        {
            var customer = GetFirstCustomer();
            var results = _service.Query
                .Where(c => c.Name.Equals(customer.Name))
                .OrderBy(c => c.CustomerId)
                .Find();

            Assert.IsNotNull(results);
            var firstResult = results.First();
            Assert.IsInstanceOfType(firstResult, typeof(Customer));
        }

        [TestMethod]
        public void FirstOrDefault_StartWith()
        {
            var result = _service.Query
                .Where(c => c.Name.StartsWith("Customer"))
                .FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            var customer1 = GetFirstCustomer();
            var customer2 = _service.Query
                .FirstOrDefault(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
        }

        [TestMethod]
        public void GetAll()
        {
            var results = _service.Query.GetAll();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        [TestMethod]
        public void Find_SelectedColumns()
        {
            var customer = GetFirstCustomer();
            var results = _service.Query
                .Where(c => c.CustomerId == customer.CustomerId)
                .Select(c => c.CustomerId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void Find_Top()
        {
            var customers = _service.Query.Top(5).Find();

            Assert.IsNotNull(customers);
            Assert.IsInstanceOfType(customers.First(), typeof(Customer));
            Assert.IsTrue(customers.Count() == 5);
        }

        [TestMethod]
        public void Count()
        {
            var count = _service.Query.Count(c => c.CustomerId);
            var result = count.FirstOrDefault();

            Assert.IsNotNull(count);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void CountAll()
        {
            var count = _service.Query.Count();

            Assert.IsNotNull(count);
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void Min()
        {
            var minCustomer = _service.Query
                .OrderBy(c => c.CustomerId)
                .Top(1)
                .FirstOrDefault();
            var min = _service.Query
                .Min(c => c.CustomerId)
                .FirstOrDefault();

            Assert.IsNotNull(min);
            Assert.IsTrue(min.GetValue<int>("Min") == minCustomer.CustomerId);
        }

        [TestMethod]
        public void Max()
        {
            var maxCustomer = _service.Query
                .OrderByDescending(c => c.CustomerId)
                .Top(1)
                .FirstOrDefault();
            var findMax = _service.Query
                .Max(c => c.CustomerId)
                .FirstOrDefault();

            Assert.IsNotNull(findMax);
            Assert.IsTrue(findMax.Max == maxCustomer.CustomerId);
        }

        [TestMethod]
        public void Sum()
        {
            var sum = _service.Query
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .Sum(c => c.CustomerCategoryId);

            Assert.IsNotNull(sum);
        }

        [TestMethod]
        public void Average()
        {
            var avg = _service.Query
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .Select(c => c.CustomerId)
                .Average(c => c.CustomerCategoryId);

            var result = avg.FirstOrDefault();

            Assert.IsNotNull(avg);
            Assert.IsTrue(result.GetValue<int>("Avg") > 0);
        }

        [TestMethod]
        public void Find_LeftJoin()
        {
            var results = _service.Query.LeftJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.FastAny());
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_WhereIsIn()
        {
            var customerIds = new object[] {5, 15, 25};
            var results = _service.Query
                .WhereIsIn(c => c.CustomerId, customerIds)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => customerIds.Contains(c.CustomerId)));
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_WhereNotIn()
        {
            var customerIds = new object[] {5, 15, 25};
            var results = _service.Query
                .WhereNotIn(c => c.CustomerId, customerIds)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => !customerIds.Contains(c.CustomerId)));
        }

        [TestMethod]
        public void Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var results = _service.Query
                    .OrderBy(c => c.CustomerCategoryId)
                    .Page(pageSize, page);

                Assert.AreEqual(pageSize, results.Count());
            }
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_WhereIsNull()
        {
            var results = _service.Query
                .Where(c => c.Name == null)
                .Find();

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_WhereIsNotNull()
        {
            var results = _service.Query
                .Where(c => c.Name != null)
                .Find();

            Assert.IsNotNull(results);
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_ComplexWhere()
        {
            var results = _service.Query
                .Where(c => (c.CustomerId >= 1)
                            && c.Name.Contains("Customer"))
                .Find();

            Assert.IsNotNull(results);
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_EqualsOperator()
        {
            var customer = GetFirstCustomer();
            var results = _service.Query
                .Where(c => c.CustomerId == customer.Id)
                .Find();

            Assert.IsNotNull(results);
        }
    }
}