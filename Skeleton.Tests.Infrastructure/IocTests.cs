using NUnit.Framework;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;

namespace Skeleton.Tests.Infrastructure
{
    [TestFixture]
    public class IocTests
    {
        private readonly IAppHost _host = new AppHost();

        [Test]
        public void Ioc_ShoudResolveDependencyOfT()
        {
            var cacheProvider = _host.Resolve<ICacheProvider>();

            Assert.IsNotNull(cacheProvider);
            Assert.IsInstanceOf(typeof(ICacheProvider), cacheProvider);
        }

        [Test]
        public void Ioc_ShoudRegisterDependencyType()
        {
            _host.RegisterType(typeof(IEntityValidator<>), typeof(CustomerValidator));
            var customerValidator = _host.Resolve<IEntityValidator<Customer>>();

            Assert.IsNotNull(customerValidator);
            Assert.IsInstanceOf(typeof(CustomerValidator), customerValidator);
        }

        [Test]
        public void Ioc_ShoudRegisterInstance()
        {
            _host.RegisterInstance(new Customer { CustomerId = 1 });
            var customer = _host.Resolve<Customer>();

            Assert.IsNotNull(customer);
            Assert.IsInstanceOf(typeof(Customer), customer);
        }
    }
}
