using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class RepositoryTests : TestBase
    {
        private readonly CustomerRepository _customerRepository;

        public RepositoryTests()
        {
            var accessorCache = Container.Resolve<ITypeAccessorCache>();
            var database = Container.Resolve<IDatabase>();

            _customerRepository = new CustomerRepository(accessorCache, database);
        }

        [TestMethod]
        public void Add()
        {
            var customer = new Customer {Name = "Customer"};
            var successed = _customerRepository.Add(customer);
            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id > 0);
            var result = _customerRepository.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void Add_Multiple()
        {
            var customers = Seeder.SeedCustomers(5);
            var successed = _customerRepository.Add(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void Delete()
        {
            var customer = _customerRepository.SelectTop(1).FirstOrDefault();
            var successed = _customerRepository.Delete(customer);
            Assert.IsTrue(successed);

            var result = _customerRepository.FirstOrDefault(customer.Id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Delete_Multiple()
        {
            var customers = _customerRepository.SelectTop(3).Find();
            var successed = _customerRepository.Delete(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void Update()
        {
            var customer = _customerRepository.SelectTop(1).FirstOrDefault();
            customer.Name = "UpdatedName";
            var successed = _customerRepository.Update(customer);
            Assert.IsTrue(successed);

            var result = _customerRepository.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Name.Equals("UpdatedName"));
        }

        [TestMethod]
        public void Update_Multiple()
        {
            var customers = _customerRepository.SelectTop(5).Find().ToList();
            foreach (var cust in customers)
                cust.Name = "Updated" + cust.Id;

            var successed = _customerRepository.Update(customers);
            Assert.IsTrue(successed);
        }
    }
}