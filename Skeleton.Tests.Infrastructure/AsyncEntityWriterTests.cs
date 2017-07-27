using NUnit.Framework;
using Skeleton.Abstraction.Orm;
using Skeleton.Core;
using Skeleton.Tests.Common;
using System.Reflection;
using System.Threading.Tasks;

namespace Skeleton.Tests.Infrastructure
{
    [TestFixture]
    public class AsyncEntityWriterTests : OrmTestBase
    {
        private readonly IAsyncEntityWriter<Customer> _writer;
        private readonly IAsyncEntityReader<Customer> _reader;

        public AsyncEntityWriterTests()
        {
            _writer = Resolver.Resolve<IAsyncEntityWriter<Customer>>();
            _reader = Resolver.Resolve<IAsyncEntityReader<Customer>>();
        }

        [Test]
        public async Task AsyncEntityWriter_AddAsync()
        {
            var customer = MemorySeeder.SeedCustomer();
            var successed = await _writer.AddAsync(customer);

            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id.IsNotZeroOrEmpty());

            var result = await _reader.FirstOrDefaultAsync(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(Customer), result);
        }

        [Test]
        public async Task AsyncEntityWriter_AddAsync_Multiple()
        {
            var customers = MemorySeeder.SeedCustomers(5);
            var successed = await _writer.AddAsync(customers);

            Assert.IsTrue(successed);
        }

        [Test]
        public async Task AsyncEntityWriter_DeleteAsync()
        {
            var customer1 = await _reader
                .Top(1)
                .FirstOrDefaultAsync();
            var successed = await _writer.DeleteAsync(customer1);
            Assert.IsTrue(successed);

            var result2 = await _reader.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNull(result2);
        }

        [Test]
        public async Task AsyncEntityWriter_DeleteAsync_Multiple()
        {
            var customers = await _reader
                .Top(3)
                .FindAsync();
            var successed = await _writer.DeleteAsync(customers);
            Assert.IsTrue(successed);
        }

        [Test]
        public async Task AsyncEntityWriter_UpdateAsync()
        {
            var customer1 = await _reader
                .Top(1)
                .FirstOrDefaultAsync();
            customer1.Name = "CustomerUpdated";
            var successed = await _writer.UpdateAsync(customer1);
            Assert.IsTrue(successed);

            var customer2 = await _reader.FirstOrDefaultAsync(customer1.Id);
            Assert.IsNotNull(customer2);
            Assert.IsTrue(customer2.Name.Equals("CustomerUpdated"));
        }

        [Test]
        public async Task AsyncEntityWriter_UpdateAsync_Multiple()
        {
            var customers = await _reader
                .Top(3)
                .FindAsync();
            var successed = await _writer.UpdateAsync(customers);
            Assert.IsTrue(successed);
        }

        [Test]
        public async Task AsyncEntityWriter_SaveAsync_ShouldAdd()
        {
            var customer = MemorySeeder.SeedCustomer();
            var successed = await _writer.SaveAsync(customer);
            Assert.IsTrue(successed);
            Assert.IsTrue(customer.Id.IsNotZeroOrEmpty());

            var result = await _reader.FirstOrDefaultAsync(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(Customer), result);
        }

        [Test]
        public async Task AsyncEntityWriter_SaveAsync_ShouldUpdate()
        {
            var customer = await _reader.Top(1).FirstOrDefaultAsync();
            Assert.IsTrue(customer.Id.IsNotZeroOrEmpty());

            customer.Name = "CustomerUpdated";
            var successed = await _writer.SaveAsync(customer);
            Assert.IsTrue(successed);

            var result = await _reader.FirstOrDefaultAsync(customer.Id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Name.Equals("CustomerUpdated"));
        }

        [Test]
        public async Task AsyncEntityWriter_SaveAsync_Multiple()
        {
            var customers = MemorySeeder.SeedCustomers(5);
            var successed = await _writer.SaveAsync(customers);
            Assert.IsTrue(successed);
        }

        [Test]
        public void AsyncEntityWriter_Dispose()
        {
            using (_writer)
            {
            }

            var fieldInfo = typeof(DisposableBase).GetField("_disposed",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(fieldInfo);
            Assert.IsTrue((bool)fieldInfo.GetValue(_writer));
        }
    }
}