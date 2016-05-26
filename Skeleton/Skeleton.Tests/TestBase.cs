using Skeleton.Common;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.DependencyResolver;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    public abstract class TestBase
    {
        protected TestBase()
        {
           Bootstrapper.Initialize();
           Register();
        }

        protected static IDependencyContainer Container
        {
            get { return Bootstrapper.Container; }
        }

        private static void Register()
        {
            Bootstrapper.Registrar
            .RegisterType(typeof(IRepository<Customer,int>), typeof(CustomerRepository))
            .RegisterType(typeof(IRepository<CustomerCategory,int>), typeof(CustomerCategoryRepository))
            .RegisterType(typeof(ICachedRepository<Customer,int>), typeof(CachedCustomerRepository))
            .RegisterType(typeof(IRepositoryAsync<Customer,int>), typeof(CustomerRepositoryAsync))
            .RegisterType(typeof(ICachedRepositoryAsync<Customer,int>), typeof(CachedCustomerRepositoryAsync));
        }
    }
}