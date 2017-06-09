using NUnit.Framework;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Infrastructure.Dependency;
using Skeleton.Tests.Common;

namespace Skeleton.Tests.Infrastructure
{
    [TestFixture]
    public class IocTests
    {
        [Test]
        public void Ioc_ShoudResolveDependencyOfT()
        {
            var cacheProvider = DependencyContainer.Instance.Resolve<ICacheProvider>();

            Assert.IsNotNull(cacheProvider);
            Assert.IsInstanceOf(typeof(ICacheProvider), cacheProvider);
        }

        [Test]
        public void Ioc_ShoudRegisterDependencyType()
        {
            DependencyContainer.Instance.Register.Type(typeof(IEntityValidator<>), typeof(CustomerValidator));
            var customerValidator = DependencyContainer.Instance.Resolve<IEntityValidator<Customer>>();

            Assert.IsNotNull(customerValidator);
            Assert.IsInstanceOf(typeof(CustomerValidator), customerValidator);
        }

        [Test]
        public void Ioc_ShoudRegisterInstance()
        {
            DependencyContainer.Instance.Register.Instance(new Customer { CustomerId = 1 });
            var customer = DependencyContainer.Instance.Resolve<Customer>();

            Assert.IsNotNull(customer);
            Assert.IsInstanceOf(typeof(Customer), customer);
        }
    }
}