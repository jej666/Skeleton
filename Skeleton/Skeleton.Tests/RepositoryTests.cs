using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class RepositoryTests : TestBase
    {
        private readonly CustomerRepository _repository;

        public RepositoryTests()
        {
            var accessorCache = Container.Resolve<ITypeAccessorCache>();
            var database = Container.Resolve<IDatabase>();

            _repository = new CustomerRepository(accessorCache, database);
        }

        //[TestMethod]
        //public void Add()
        //{
        //    var customer = new Customer { Name = "Foo" };
        //    var successed = _repository.Add(customer);
        //    Assert.IsTrue(successed);
        //    Assert.IsTrue(customer.Id > 0);
        //    var result = _repository.FirstOrDefault(customer.Id);
        //    Assert.IsNotNull(result);
        //    Assert.IsInstanceOfType(result, typeof(Customer));
        //}

        //[TestMethod]
        //public void Add_Multiple()
        //{
        //    var customers = new CustomerSeeder().Seed(5);
        //    var successed = _repository.Add(customers);
        //    Assert.IsTrue(successed);
        //}

        //[TestMethod]
        //public void Delete()
        //{
        //    var customer = _repository.Query.SelectTop(1).FirstOrDefault();
        //    var successed = _repository.Delete(customer);
        //    Assert.IsTrue(successed);

        //    var result = _repository.FirstOrDefault(customer.Id);
        //    Assert.IsNull(result);
        //}

        //[TestMethod]
        //public void Delete_Multiple()
        //{
        //    var customers = _repository.Query.SelectTop(3).Find();
        //    var successed = _repository.Delete(customers);
        //    Assert.IsTrue(successed);
        //}

        [TestMethod]
        public void Find_ByExpression()
        {
            var customer = _repository.SelectTop(1).FirstOrDefault();
            var results = _repository.Where(c => c.Name.Equals(customer.Name))
                                     .OrderBy(c => c.CustomerId)
                                     .Find();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        [TestMethod]
        public void FirstOrDefault_ByExpression()
        {
            var result = _repository.Where(c => c.Name.Equals("Foo")).FirstOrDefault();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            var customer1 = _repository.SelectTop(1).FirstOrDefault();
            var customer2 = _repository.FirstOrDefault(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsInstanceOfType(customer2, typeof(Customer));
            Assert.AreEqual(customer1, customer2);
        }

        [TestMethod]
        public void GetAll()
        {
            var results = _repository.GetAll();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(Customer));
        }

        //[TestMethod]
        //public void Update()
        //{
        //    var customer = _repository.Query.SelectTop(1).FirstOrDefault();
        //    customer.Name = "UpdatedName";
        //    var successed = _repository.Update(customer);
        //    Assert.IsTrue(successed);

        //    var result = _repository.FirstOrDefault(customer.Id);
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Name.Equals("UpdatedName"));
        //}

        //[TestMethod]
        //public void Update_Multiple()
        //{
        //    var customers = _repository.Query.SelectTop(5).Find();
        //    foreach (var cust in customers)
        //        cust.Name = "Updated" + cust.Id;

        //    var successed = _repository.Update(customers);
        //    Assert.IsTrue(successed);
        //}

        [TestMethod]
        public void SelectTop()
        {
            var customers = _repository.SelectTop(5).Find();
            Assert.IsNotNull(customers);
            Assert.IsInstanceOfType(customers.First(), typeof(Customer));
            Assert.IsTrue(customers.Count() == 5);
        }

        //[TestMethod]
        //public void SelectCount()
        //{
        //    var count = _repository.Aggregate.Count(c => c.CustomerId).As<int>();
        //    Assert.IsNotNull(count);
        //    Assert.IsTrue(count > 0);
        //}

        [TestMethod]
        public void CustomQuery()
        {
            var customerId = _repository.SelectTop(1).FirstOrDefault().CustomerId;
            var customer1 = _repository.Where(c => c.CustomerId == customerId).FirstOrDefault();
            var customer2 = _repository.Where(c => c.CustomerId == customerId).Find().FirstOrDefault();
            Assert.IsNotNull(customer1);
            Assert.IsNotNull(customer2);
            Assert.AreEqual(customer1, customer2);
        }

    }
}