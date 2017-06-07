using NUnit.Framework;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Startup;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;

namespace Skeleton.Tests.Infrastructure
{
    [TestFixture]
    public class IocTests
    {
        private readonly IBootstrapper _bootstrapper = new Bootstrapper();

        [Test]
        public void Ioc_ShoudResolveDependencyOfT()
        {
            var cacheProvider = _bootstrapper.Resolve<ICacheProvider>();

            Assert.IsNotNull(cacheProvider);
            Assert.IsInstanceOf(typeof(ICacheProvider), cacheProvider);
        }

        [Test]
        public void Ioc_ShoudRegisterDependencyType()
        {
            _bootstrapper.RegisterType(typeof(IEntityValidator<>), typeof(CustomerValidator));
            var customerValidator = _bootstrapper.Resolve<IEntityValidator<Customer>>();

            Assert.IsNotNull(customerValidator);
            Assert.IsInstanceOf(typeof(CustomerValidator), customerValidator);
        }

        [Test]
        public void Ioc_ShoudRegisterInstance()
        {
            _bootstrapper.RegisterInstance(new Customer { CustomerId = 1 });
            var customer = _bootstrapper.Resolve<Customer>();

            Assert.IsNotNull(customer);
            Assert.IsInstanceOf(typeof(Customer), customer);
        }
    }
}
