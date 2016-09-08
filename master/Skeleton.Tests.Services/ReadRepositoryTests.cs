using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class ReadRepositoryTests : TestBase
    {
        private readonly ICrudRepository<Customer, CustomerDto> _repository;

        public ReadRepositoryTests()
        {
            _repository = Container.Resolve<ICrudRepository<Customer, CustomerDto>>();

            SqlDbSeeder.SeedCustomers();
        }

        private Customer GetFirstCustomer()
        {
            return _repository.Query
                .Top(1)
                .FirstOrDefault();
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_EqualsMethod()
        {
            var customer = GetFirstCustomer();
            var results = _repository.Query
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
            var result = _repository.Query
                .Where(c => c.Name.StartsWith("Customer"))
                .FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            var customer1 = GetFirstCustomer();
            var customer2 = _repository.Query
                .FirstOrDefault(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
        }

        [TestMethod]
        public void GetAll()
        {
            var results = _repository.Query.GetAll();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        [TestMethod]
        public void Find_SelectedColumns()
        {
            var customer = GetFirstCustomer();
            var results = _repository.Query
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
            var customers = _repository.Query.Top(5).Find();

            Assert.IsNotNull(customers);
            Assert.IsInstanceOfType(customers.First(), typeof(Customer));
            Assert.IsTrue(customers.Count() == 5);
        }

        [TestMethod]
        public void Count()
        {
            var count = _repository.Query.Count(c => c.CustomerId);

            Assert.IsNotNull(count);

            var result = count.FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void CountAll()
        {
            var count = _repository.Query.Count();

            Assert.IsNotNull(count);
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void Min()
        {
            var minCustomer = _repository.Query
                .OrderBy(c => c.CustomerId)
                .Top(1)
                .FirstOrDefault();
            var min = _repository.Query
                .Min(c => c.CustomerId)
                .FirstOrDefault();

            Assert.IsNotNull(min);
            Assert.IsTrue(min.Min == minCustomer.CustomerId);
        }

        [TestMethod]
        public void Max()
        {
            var maxCustomer = _repository.Query
                .OrderByDescending(c => c.CustomerId)
                .Top(1)
                .FirstOrDefault();
            var findMax = _repository.Query
                .Max(c => c.CustomerId)
                .FirstOrDefault();

            Assert.IsNotNull(findMax);
            Assert.IsTrue(findMax.Max == maxCustomer.CustomerId);
        }

        [TestMethod]
        public void Sum()
        {
            var sum = _repository.Query
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .Sum(c => c.CustomerCategoryId);

            var result = sum.FirstOrDefault();

            Assert.IsNotNull(sum);
            Assert.IsTrue((result != null) && (result.Sum > 0));
        }

        [TestMethod]
        public void Average()
        {
            var avg = _repository.Query
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .Select(c => c.CustomerId)
                .Average(c => c.CustomerCategoryId);

            var result = avg.FirstOrDefault();

            Assert.IsNotNull(avg);
            Assert.IsTrue((result != null) && (result.Avg > 0));
        }

        [TestMethod]
        public void Find_LeftJoin()
        {
            var results = _repository.Query.LeftJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_WhereIsIn()
        {
            var customerIds = new object[] { 5, 15, 25 };
            var results = _repository.Query
                .WhereIsIn(c => c.CustomerId, customerIds)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => customerIds.Contains(c.CustomerId)));
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_WhereNotIn()
        {
            var customerIds = new object[] { 5, 15, 25 };
            var results = _repository.Query
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
                var results = _repository.Query
                    .OrderBy(c => c.CustomerCategoryId)
                    .Page(pageSize, page);

                Assert.AreEqual(pageSize, results.Count());
            }
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_WhereIsNull()
        {
            var results = _repository.Query
                .Where(c => c.Name == null)
                .Find();

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_WhereIsNotNull()
        {
            var results = _repository.Query
                .Where(c => c.Name != null)
                .Find();

            Assert.IsNotNull(results);
        }

        [TestMethod]
        [TestCategory("Where")]
        public void Find_ComplexWhere()
        {
            var results = _repository.Query
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
            var results = _repository.Query
                .Where(c => c.CustomerId.Equals(customer.Id))
                .Find();

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
            Assert.IsTrue((bool)fieldInfo.GetValue(_repository.Query));
        }
    }
}