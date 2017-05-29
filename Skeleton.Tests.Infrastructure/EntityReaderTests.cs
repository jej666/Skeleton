using NUnit.Framework;
using Skeleton.Abstraction.Orm;
using Skeleton.Common;
using Skeleton.Tests.Common;
using System;
using System.Linq;
using System.Reflection;

namespace Skeleton.Tests.Infrastructure
{
    [TestFixture]
    public class EntityReaderTests : OrmTestBase
    {
        private readonly IEntityReader<Customer> _reader;

        public EntityReaderTests()
        {
            _reader = Resolver.Resolve<IEntityReader<Customer>>();
        }

        private Customer GetFirstCustomer()
        {
            return _reader
                .Top(1)
                .FirstOrDefault();
        }

        [Test]
        public void EntityReader_Find_EqualsMethod()
        {
            var customer = GetFirstCustomer();
            var results = _reader
                .Where(c => c.Name.Equals(customer.Name))
                .OrderBy(c => c.CustomerId)
                .Find();

            Assert.IsNotNull(results);
            var firstResult = results.First();
            Assert.IsInstanceOf(typeof(Customer), firstResult);
        }

        [Test]
        public void EntityReader_FirstOrDefault_StartWith()
        {
            var result = _reader
                .Where(c => c.Name.StartsWith("Customer", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(Customer), result);
        }

        [Test]
        public void EntityReader_FirstOrDefault_ById()
        {
            var customer1 = GetFirstCustomer();
            var customer2 = _reader
                .FirstOrDefault(customer1.Id);

            Assert.IsNotNull(customer2);
            Assert.IsInstanceOf(typeof(Customer), customer2);
            Assert.AreEqual(customer1, customer2);
        }

        [Test]
        public void EntityReader_FirstOrDefault_With_Wrong_Id()
        {
            var customer = _reader.FirstOrDefault(100000);

            Assert.IsNull(customer);
        }

        [Test]
        public void EntityReader_GetAll()
        {
            var results = _reader.Find();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(Customer), results.First());
        }

        [Test]
        public void EntityReader_Find_Selected_Columns()
        {
            var customer = GetFirstCustomer();
            var results = _reader
                .Where(c => c.CustomerId == customer.CustomerId)
                .Select(c => c.CustomerId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(Customer), results.First());
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void EntityReader_Find_Top()
        {
            var customers = _reader.Top(5).Find();

            Assert.IsNotNull(customers);
            Assert.IsInstanceOf(typeof(Customer), customers.First());
            Assert.IsTrue(customers.Count() == 5);
        }

        [Test]
        public void EntityReader_Count()
        {
            var count = _reader.Count(c => c.CustomerId);

            Assert.IsNotNull(count);

            var result = count.FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.CountCustomerId > 0);
        }

        [Test]
        public void EntityReader_Count_All()
        {
            var count = _reader.Count();

            Assert.IsNotNull(count);
            Assert.IsTrue(count > 0);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void EntityReader_Sum()
        {
            var sum = _reader
                .OrderBy(c => c.CustomerId)
                .GroupBy(c => c.CustomerId)
                .Sum(c => c.CustomerId);

            var result = sum.FirstOrDefault();

            Assert.IsNotNull(sum);
            Assert.IsTrue((result != null) && (result.SumCustomerId > 0));
        }

        [Test]
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

        [Test]
        public void EntityReader_Find_LeftJoin()
        {
            var results = _reader.LeftJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void EntityReader_Find_RightJoin()
        {
            var results = _reader.RightJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void EntityReader_Find_InnerJoin()
        {
            var results = _reader.InnerJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void Find_RightJoin_Distinct()
        {
            var results = _reader.RightJoin<CustomerCategory>(
                    (customer, category) =>
                            customer.CustomerCategoryId == category.CustomerCategoryId)
                .Distinct(customer => customer.CustomerId)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void EntityReader_Find_Where_Is_In()
        {
            var customerIds = new object[] { 5, 15, 25 };
            var results = _reader
                .WhereIsIn(c => c.CustomerId, customerIds)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => customerIds.Contains(c.CustomerId)));
        }

        [Test]
        public void EntityReader_Find_Where_Not_In()
        {
            var customerIds = new object[] { 5, 15, 25 };
            var results = _reader
                .WhereNotIn(c => c.CustomerId, customerIds)
                .Find();

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(c => !customerIds.Contains(c.CustomerId)));
        }

        [Test]
        public void EntityReader_Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var results = _reader
                    .OrderBy(c => c.CustomerCategoryId)
                    .Query(new Query
                    {
                        PageSize = pageSize,
                        PageNumber = page
                    });

                Assert.IsTrue(results.Count() <= pageSize);
            }
        }

        [Test]
        public void EntityReader_Find_Where_Is_Null()
        {
            var results = _reader
                .Where(c => c.Name == null)
                .Find();

            Assert.IsNotNull(results);
        }

        [Test]
        public void EntityReader_Find_Where_Is_Not_Null()
        {
            var results = _reader
                .Where(c => c.Name != null)
                .Find();

            Assert.IsNotNull(results);
        }

        [Test]
        public void EntityReader_Find_Where_Complex()
        {
            var results = _reader
                .Where(c => (c.CustomerId >= 1)
                            && c.Name.Contains("Customer"))
                .Find();

            Assert.IsNotNull(results);
        }

        [Test]
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

        [Test]
        public void EntityReader_Find_Where_Not_Found()
        {
            var results = _reader
                .Where(c => c.Name.StartsWith("Jerome", StringComparison.OrdinalIgnoreCase))
                .Find();

            Assert.IsFalse(results.Any());
        }

        [Test]
        public void EntityReader_FirstOrDefault_Where_Not_Found()
        {
            var result = _reader
                .Where(c => c.Name.StartsWith("Jerome", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            Assert.IsNull(result);
        }
    }
}