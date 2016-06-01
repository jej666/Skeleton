using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Core.Repository;
using Skeleton.Tests.Infrastructure;
using Skeleton.Core.Service;

namespace Skeleton.Tests
{
    [TestClass]
    public class ServiceTests : TestBase
    {
        private readonly IService<Customer, int> _service;

        public ServiceTests()
        {
            _service = Container.Resolve<IService<Customer, int>>();

            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public void Add()
        {
            var customer = new Customer {Name = "Customer"};
            var successed = _service.Repository.Add(customer);
            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id > 0);
            var result = _service.Repository.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void Add_Multiple()
        {
            var customers = SqlDbSeeder.SeedCustomers(5);
            var successed = _service.Repository.Add(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void Delete()
        {
            var customer = _service.Repository
                .SelectTop(1)
                .FirstOrDefault();
            var successed = _service.Repository.Delete(customer);
            Assert.IsTrue(successed);

            var result = _service.Repository.FirstOrDefault(customer.Id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Delete_Multiple()
        {
            var customers = _service.Repository
                .SelectTop(3)
                .Find();
            var successed = _service.Repository.Delete(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void Update()
        {
            var customer = _service.Repository
                .SelectTop(1)
                .FirstOrDefault();
            customer.Name = "UpdatedName";
            var successed = _service.Repository.Update(customer);
            Assert.IsTrue(successed);

            var result = _service.Repository.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Name.Equals("UpdatedName"));
        }

        [TestMethod]
        public void Update_Multiple()
        {
            var customers = _service.Repository
                .SelectTop(5)
                .Find()
                .ToList();
            foreach (var cust in customers)
                cust.Name = "Updated" + cust.Id;

            var successed = _service.Repository.Update(customers);
            Assert.IsTrue(successed);
        }
    }
}