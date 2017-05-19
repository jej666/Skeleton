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
        [Test]
        public void Ioc_ShoudResolveDependencyOfT()
        {
            var cacheProvider = Bootstrapper.Resolver.Resolve<ICacheProvider>();

            Assert.IsNotNull(cacheProvider);
            Assert.IsInstanceOf(typeof(ICacheProvider), cacheProvider);
        }

        [Test]
        public void Ioc_ShoudRegisterDependencyType()
        {
            Bootstrapper.Registrar.RegisterType(typeof(IEntityValidator<>), typeof(CustomerValidator));
            var customerValidator = Bootstrapper.Resolver.Resolve<IEntityValidator<Customer>>();

            Assert.IsNotNull(customerValidator);
            Assert.IsInstanceOf(typeof(CustomerValidator), customerValidator);
        }

        [Test]
        public void Ioc_ShoudRegisterInstance()
        {
            Bootstrapper.Registrar.RegisterInstance(new Customer { CustomerId = 1 });
            var customer = Bootstrapper.Resolver.Resolve<Customer>();

            Assert.IsNotNull(customer);
            Assert.IsInstanceOf(typeof(Customer), customer);
        }
    }
}
