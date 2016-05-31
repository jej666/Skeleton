using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Common.Extensions;
using Skeleton.Core.Repository;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class ReadOnlyRepositoryTests : TestBase
    {
        private readonly IRepository<Customer, int> _customerRepository;

        public ReadOnlyRepositoryTests()
        {
            _customerRepository = Container.Resolve<IRepository<Customer, int>>();

            Seeder.SeedCustomers();
        }

        [TestMethod]
        public void Find_ByExpression()
        {
            var customer = _customerRepository
                .SelectTop(1)
                .FirstOrDefault();
            var results = _customerRepository
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
            var result = _customerRepository
                .Where(c => c.Name.Equals("Foo"))
                .FirstOrDefault();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            var customer1 = _customerRepository.SelectTop(1).FirstOrDefault();
            var customer2 = _customerRepository.FirstOrDefault(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
        }

        [TestMethod]
        public void GetAll()
        {
            var results = _customerRepository.GetAll();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        [TestMethod]
        public void SelectTop()
        {
            var customers = _customerRepository
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
            var count = _customerRepository.Count(c => c.CustomerId);
            Assert.IsNotNull(count);
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void SelectMin()
        {
            var minCustomer = _customerRepository
                .OrderBy(c => c.CustomerId)
                .SelectTop(1)
                .FirstOrDefault();
            var min = _customerRepository.Min(c => c.CustomerId);
            Assert.IsNotNull(min);
            Assert.IsTrue(min == minCustomer.CustomerId);
        }

        [TestMethod]
        public void SelectMax()
        {
            var maxCustomer = _customerRepository
                .OrderByDescending(c => c.CustomerId)
                .SelectTop(1)
                .FirstOrDefault();
            var max = _customerRepository.Max(c => c.CustomerId);
            Assert.IsNotNull(max);
            Assert.IsTrue(max == maxCustomer.CustomerId);
        }

        [TestMethod]
        public void LeftJoin()
        {
            var results = _customerRepository.LeftJoin<CustomerCategory>(
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

            var results = _customerRepository
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

            var results = _customerRepository
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
                var results = _customerRepository
                    .OrderBy(c => c.CustomerCategoryId)
                    .Page(pageSize, page)
                    .ToList();

                Assert.AreEqual(pageSize, results.Count);
            }
        }
    }
}