using System;
using Microsoft.Practices.Unity;
using Skeleton.Common;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Abstraction;

namespace Skeleton.Infrastructure.DependencyResolver
{
    public static class Bootstrapper
    {
        private static readonly Lazy<IUnityContainer> UnityContainer =
            new Lazy<IUnityContainer>(() => new UnityContainer());

        private static readonly Lazy<IDependencyResolver> ContainerWrapper =
            new Lazy<IDependencyResolver>(() => new DependencyResolver(UnityContainer.Value));

        private static readonly Lazy<IDependencyRegistrar> RegistrarWrapper =
            new Lazy<IDependencyRegistrar>(() => new DependencyRegistrar(UnityContainer.Value));

        public static IDependencyResolver Resolver
        {
            get { return ContainerWrapper.Value; }
        }

        public static IDependencyRegistrar Registrar
        {
            get { return RegistrarWrapper.Value; }
        }

        public static IUnityContainer Container
        {
            get { return UnityContainer.Value; }
        }

        public static void Initialize()
        {
            UnityContainer.Value.RegisterInstance(Resolver);

            UnityContainer.Value
                .AddExtension(new CommonModuleExtension())
                .AddExtension(new DataModuleExtension())
                .AddExtension(new RepositoryModuleExtension())
                .AddExtension(new ServiceModuleExtension());
        }

        public static void UseDatabase(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            var builder = Resolver.Resolve<IDatabaseConfigurationBuilder>();
            UnityContainer.Value.RegisterInstance(configurator.Invoke(builder));
        }
    }
}