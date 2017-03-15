using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Orm;
using Skeleton.Common;
using Skeleton.Tests.Common;
using System;

namespace Skeleton.Tests.Infrastructure
{
    [TestClass]
    public class EntityReaderTests : OrmTestBase
    {
        private readonly IEntityReader<Customer> _reader;

        public EntityReaderTests()
        {
            _reader = Container.Resolve<IEntityReader<Customer>>();

            SqlDbSeeder.SeedCustomers();
        }

        private Customer GetFirstCustomer()
        {
            return _reader
                .Top(1)
                .FirstOrDefault();
        }

        [TestMethod]
        public void EntityReader_Find_EqualsMethod()
        {
            var customer = GetFirstCustomer();
            var results = _reader
                .Where(c => c.Name.Equals(customer.Name))
                .OrderBy(c => c.CustomerId)
                .Find();

            Assert.IsNotNull(results);
            var firstResult = results.First();
            Assert.IsInstanceOfType(firstResult, typeof(Customer));
        }

        [TestMethod]
        public void EntityReader_FirstOrDefault_StartWith()
        {
            var result = _reader
                .Where(c => c.Name.StartsWith("Customer", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void EntityReader_FirstOrDefault_ById()
        {
            var customer1 = GetFirstCustomer();
            var customer2 = _reader
                .FirstOrDefault(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
        }

        [TestMethod]
        public void EntityReader_FirstOrDefault_With_Wrong_Id()
        {
            var customer = _reader.FirstOrDefault(100000);

            Assert.IsNull(customer);
        }

        [TestMethod]
        public void EntityReader_GetAll()
        {
            var results = _reader.Find();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        [TestMethod]
        public void EntityReader_Find_Selected_Columns()
        {
            var customer = GetFirstCustomer();
            var results = _reader
                .Where(c => c.CustomerId == customer.CustomerId)
                .Select(c => c.CustomerId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void EntityReader_Find_Top()
        {
            var customers = _reader.Top(5).Find();

            Assert.IsNotNull(customers);
            Assert.IsInstanceOfType(customers.First(), typeof(Customer));
            Assert.IsTrue(customers.Count() == 5);
        }

        [TestMethod]
        public void EntityReader_Count()
        {
            var count = _reader.Count(c => c.CustomerId);

            Assert.IsNotNull(count);

            var result = count.FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.CountCustomerId > 0);
        }

        [TestMethod]
        public void EntityReader_Count_All()
        {
            var count = _reader.Count();

            Assert.IsNotNull(count);
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void EntityReader_Min()
        {
            var minCustomer = _reader
                .OrderBy(c => c.CustomerId)
                .Top(1)
                .FirstOrDefault();
            var min = _reader
                .Min(c => c.CustomerId)
                .FirstOrDefault();

            Assert.IsNotNull(min);
            Assert.IsTrue(min.MinCustomerId == minCustomer.CustomerId);
        }

        [TestMethod]
        public void EntityReader_Max()
        {
            var maxCustomer = _reader
                .OrderByDescending(c => c.CustomerId)
                .Top(1)
                .FirstOrDefault();
            var findMax = _reader
                .Max(c => c.CustomerId)
                .FirstOrDefault();

            Assert.IsNotNull(findMax);
            Assert.IsTrue(findMax.MaxCustomerId == maxCustomer.CustomerId);
        }

        [TestMethod]
        public void EntityReader_Sum()
        {
            var sum = _reader
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .Sum(c => c.CustomerCategoryId);

            var result = sum.FirstOrDefault();

            Assert.IsNotNull(sum);
            Assert.IsTrue((result != null) && (result.SumCustomerCategoryId > 0));
        }

        [TestMethod]
        public void EntityReader_Average()
        {
            var avg = _reader
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .Average(c => c.CustomerId);

            var result = avg.FirstOrDefault();

            Assert.IsNotNull(avg);
            Assert.IsTrue((result != null) && (result.AvgCustomerId > 0));
        }

        [TestMethod]
        public void EntityReader_Find_LeftJoin()
        {
            var results = _reader.LeftJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void EntityReader_Find_RightJoin()
        {
            var results = _reader.RightJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void EntityReader_Find_InnerJoin()
        {
            var results = _reader.InnerJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void Find_RightJoin_Distinct()
        {
            var results = _reader.InnerJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Distinct(customer => customer.CustomerId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void EntityReader_Find_Where_Is_In()
        {
            var customerIds = new object[] { 5, 15, 25 };
            var results = _reader
                .WhereIsIn(c => c.CustomerId, customerIds)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => customerIds.Contains(c.CustomerId)));
        }

        [TestMethod]
        public void EntityReader_Find_Where_Not_In()
        {
            var customerIds = new object[] { 5, 15, 25 };
            var results = _reader
                .WhereNotIn(c => c.CustomerId, customerIds)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => !customerIds.Contains(c.CustomerId)));
        }

        [TestMethod]
        public void EntityReader_Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var results = _reader
                    .OrderBy(c => c.CustomerCategoryId)
                    .Page(pageSize, page);

                Assert.IsTrue(results.Count() <= pageSize);
            }
        }

        [TestMethod]
        public void EntityReader_Find_Where_Is_Null()
        {
            var results = _reader
                .Where(c => c.Name == null)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void EntityReader_Find_Where_Is_Not_Null()
        {
            var results = _reader
                .Where(c => c.Name != null)
                .Find();

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void EntityReader_Find_Where_Complex()
        {
            var results = _reader
                .Where(c => (c.CustomerId >= 1)
                            && c.Name.Contains("Customer"))
                .Find();

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void EntityReader_Dispose()
        {
            using (_reader)
            {
            }

            var fieldInfo = typeof(DisposableBase).GetField("_disposed",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(fieldInfo);
            Assert.IsTrue((bool)fieldInfo.GetValue(_reader));
        }

        [TestMethod]
        public void EntityReader_Find_Where_Not_Found()
        {
            var results = _reader
                .Where(c => c.Name.StartsWith("Jerome", StringComparison.OrdinalIgnoreCase))
                .Find();

            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void EntityReader_FirstOrDefault_Where_Not_Found()
        {
            var result = _reader
                .Where(c => c.Name.StartsWith("Jerome", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            Assert.IsNull(result);
        }
    }
}