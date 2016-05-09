using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Tests.Infrastructure;
using System.Linq;

namespace Skeleton.Tests
{
    [TestClass]
    public class RepositoryTests : TestBase
    {
        private readonly CustomerRepository repository;

        public RepositoryTests()
        {
            var accessorCache = Container.Resolve<ITypeAccessorCache>();
            var database = Container.Resolve<IDatabase>();

            repository = new CustomerRepository(accessorCache, database);
        }

        [TestMethod]
        public void Add()
        {
            var customer = new Customer() { Name = "Foo" };
            var successed = repository.Add(customer);
            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id > 0);
            var result = repository.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void Add_Multiple()
        {
            var customers = new CustomerSeeder().Seed(5);
            var successed = repository.Add(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void Delete()
        {
            var customer = repository.Query.SelectTop(1).FirstOrDefault();
            var successed = repository.Delete(customer);
            Assert.IsTrue(successed);

            var result = repository.FirstOrDefault(customer.Id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Delete_Multiple()
        {
            var customers = repository.Query.SelectTop(3).Find();
            var successed = repository.Delete(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void Find_ByExpression()
        {
            var customer = repository.Query.SelectTop(1).FirstOrDefault();
            var results = repository.Find(
                c => c.Name.Equals(customer.Name),
                c => c.CustomerId);
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        [TestMethod]
        public void FirstOrDefault_ByExpression()
        {
            var result1 = repository.FirstOrDefault(c => c.Name.Equals("Foo"));
            Assert.IsNotNull(result1);
            Assert.IsInstanceOfType(result1, typeof(Customer));

            var result2 = repository.FirstOrDefault(c => c.Name.Equals("Foo"));
            Assert.AreEqual(result1, result2);
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            var customer1 = repository.Query.SelectTop(1).FirstOrDefault();
            var customer2 = repository.FirstOrDefault(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
        }

        [TestMethod]
        public void GetAll()
        {
            var results = repository.GetAll();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        [TestMethod]
        public void Update()
        {
            var customer = repository.Query.SelectTop(1).FirstOrDefault();
            customer.Name = "UpdatedName";
            var successed = repository.Update(customer);
            Assert.IsTrue(successed);

            var result = repository.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Name.Equals("UpdatedName"));
        }

        [TestMethod]
        public void Update_Multiple()
        {
            var customers = repository.Query.SelectTop(5).Find();
            foreach (var cust in customers)
                cust.Name = "Updated" + cust.Id;

            var successed = repository.Update(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void SelectTop()
        {
            var customers = repository.Query.SelectTop(5).Find();
            Assert.IsNotNull(customers);
            Assert.IsInstanceOfType(customers.First(), typeof(Customer));
            Assert.IsTrue(customers.Count() == 5);
        }

        [TestMethod]
        public void SelectCount()
        {
            var count = repository.Aggregate.Count(c => c.CustomerId).As<int>();
            Assert.IsNotNull(count);
            Assert.IsTrue(count> 0);
        }
    }
}