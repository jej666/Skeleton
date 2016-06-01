using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Common.Extensions;
using Skeleton.Tests.Infrastructure;
using Skeleton.Core.Service;
using Skeleton.Core.Domain;

namespace Skeleton.Tests
{
    [TestClass]
    public class ReadOnlyServiceTests : TestBase
    {
        private readonly IService<Customer, int> _service;

        public ReadOnlyServiceTests()
        {
            _service = Container.Resolve<IService<Customer, int>>();

            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public void Find_ByExpression()
        {
            var customer = _service.Repository
                .SelectTop(1)
                .FirstOrDefault();
            var results = _service.Repository
                .Where(c => c.Name.Equals(customer.Name))
                .OrderBy(c => c.CustomerId)
                .Find()
                .ToList();

            Assert.IsNotNull(results);
            var firstResult = results.First();
            Assert.IsInstanceOfType(firstResult, typeof(Customer));
        }

        [TestMethod]
        public void FirstOrDefault_ByExpression()
        {
            var result = _service.Repository
                .Where(c => c.Name.StartsWith("Customer"))
                .FirstOrDefault();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            var customer1 = _service.Repository
                .SelectTop(1)
                .FirstOrDefault();
            var customer2 = _service.Repository
                .FirstOrDefault(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
        }

        [TestMethod]
        public void GetAll()
        {
            var results = _service.Repository.GetAll();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        [TestMethod]
        public void SelectTop()
        {
            var customers = _service.Repository
                .SelectTop(5)
                .Find()
                .ToList();
            Assert.IsNotNull(customers);
            Assert.IsInstanceOfType(customers.First(), typeof(Customer));
            Assert.IsTrue(customers.Count == 5);
        }

        [TestMethod]
        public void SelectCount()
        {
            var count = _service.Repository.Count(c => c.CustomerId);
            Assert.IsNotNull(count);
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void SelectMin()
        {
            var minCustomer = _service.Repository
                .OrderBy(c => c.CustomerId)
                .SelectTop(1)
                .FirstOrDefault();
            var min = _service.Repository
                .Min(c => c.CustomerId);
            Assert.IsNotNull(min);
            Assert.IsTrue(min == minCustomer.CustomerId);
        }

        [TestMethod]
        public void SelectMax()
        {
            var maxCustomer = _service.Repository
                .OrderByDescending(c => c.CustomerId)
                .SelectTop(1)
                .FirstOrDefault();
            var max = _service.Repository
                .Max(c => c.CustomerId);
            Assert.IsNotNull(max);
            Assert.IsTrue(max == maxCustomer.CustomerId);
        }

        [TestMethod]
        public void LeftJoin()
        {
            var results = _service.Repository.LeftJoin<CustomerCategory>(
                (customer, category) =>
                    customer.CustomerCategoryId == category.CustomerCategoryId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.FastAny());
        }

        [TestMethod]
        public void WhereIsIn()
        {
            var customerIds = new object[] {5, 15, 25};

            var results = _service.Repository
                .WhereIsIn(c => c.CustomerId, customerIds)
                .Find()
                .ToList();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => customerIds.Contains(c.CustomerId)));
        }

        [TestMethod]
        public void WhereNotIn()
        {
            var customerIds = new object[] {5, 15, 25};

            var results = _service.Repository
                .WhereNotIn(c => c.CustomerId, customerIds)
                .Find()
                .ToList();

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
                var results = _service.Repository
                    .OrderBy(c => c.CustomerCategoryId)
                    .Page(pageSize, page)
                    .ToList();

                Assert.AreEqual(pageSize, results.Count);
            }
        }
    }
}