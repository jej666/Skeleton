using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Orm;
using Skeleton.Common;
using Skeleton.Tests.Common;

namespace Skeleton.Tests.Infrastructure
{
    [TestClass]
    public class EntityWriterTests : OrmTestBase
    {
        private readonly IEntityWriter<Customer> _writer;
        private readonly IEntityReader<Customer> _reader;

        public EntityWriterTests()
        {
            _writer = Container.Resolve<IEntityWriter<Customer>>();
            _reader = Container.Resolve<IEntityReader<Customer>>();

            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public void EntityWriter_Add()
        {
            var customer = MemorySeeder.SeedCustomer();
            var successed = _writer.Add(customer);
            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id.IsNotZeroOrEmpty());

            var result = _reader.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void EntityWriter_Add_Multiple()
        {
            var customers = MemorySeeder.SeedCustomers(5);
            var successed = _writer.Add(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void EntityWriter_Delete()
        {
            var customer = _reader
                .Top(1)
                .FirstOrDefault();
            var successed = _writer.Delete(customer);
            Assert.IsTrue(successed);

            var result = _reader.FirstOrDefault(customer.Id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void EntityWriter_Delete_Multiple()
        {
            var customers = _reader
                .Top(3)
                .Find();
            var successed = _writer.Delete(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void EntityWriter_Update()
        {
            var customer = _reader
                .Top(1)
                .FirstOrDefault();
            customer.Name = "CustomerUpdated";
            customer.CustomerCategoryId = new Random().Next(0, 10);
            var successed = _writer.Update(customer);
            Assert.IsTrue(successed);

            var result = _reader.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Name.Equals("CustomerUpdated"));
        }

        [TestMethod]
        public void EntityWriter_Update_Multiple()
        {
            var customers = _reader
                .Top(5)
                .Find()
                .ToList();
            foreach (var cust in customers)
                cust.Name = "CustomerUpdated" + cust.Id;

            var successed = _writer.Update(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void EntityWriter_Save_Should_Add()
        {
            var customer = MemorySeeder.SeedCustomer();
            var successed = _writer.Save(customer);
            Assert.IsTrue(successed);
            Assert.IsFalse(customer.Id.IsZeroOrEmpty());

            var result = _reader.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Customer));
        }

        [TestMethod]
        public void EntityWriter_Save_Should_Update()
        {
            var customer = _reader.Top(1).FirstOrDefault();
            Assert.IsFalse(customer.Id.IsZeroOrEmpty());

            customer.Name = "CustomerUpdated";
            var successed = _writer.Save(customer);
            Assert.IsTrue(successed);

            var result = _reader.FirstOrDefault(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Name.Equals("CustomerUpdated"));
        }

        [TestMethod]
        public void EntityWriter_Save_Multiple()
        {
            var customers = MemorySeeder.SeedCustomers(5);
            var successed = _writer.Save(customers);
            Assert.IsTrue(successed);
        }

        [TestMethod]
        public void EntityWriter_Dispose()
        {
            using (_writer)
            {
            }

            var fieldInfo = typeof(DisposableBase).GetField("_disposed",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(fieldInfo);
            Assert.IsTrue((bool) fieldInfo.GetValue(_writer));
        }
    }
}