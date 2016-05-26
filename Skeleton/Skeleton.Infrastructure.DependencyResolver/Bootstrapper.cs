using System;
using Microsoft.Practices.Unity;
using Skeleton.Common;

namespace Skeleton.Infrastructure.DependencyResolver
{
    public static class Bootstrapper
    {
        private static readonly Lazy<IUnityContainer> UnityContainer =
            new Lazy<IUnityContainer>(() => new UnityContainer());

        private static readonly Lazy<IDependencyContainer> ContainerWrapper =
            new Lazy<IDependencyContainer>(() => new DependencyContainer(UnityContainer.Value));

        private static readonly Lazy<IDependencyRegistrar> RegistrarWrapper =
            new Lazy<IDependencyRegistrar>(() => new DependencyRegistrar(UnityContainer.Value));

        public static IDependencyContainer Container
        {
            get { return ContainerWrapper.Value; }
        }

        public static IDependencyRegistrar Registrar
        {
            get { return RegistrarWrapper.Value; }
        }

        public static void Initialize()
        {
            UnityContainer.Value.RegisterInstance(Container);

            UnityContainer.Value
                .AddExtension(new CommonModuleExtension())
                .AddExtension(new DataModuleExtension())
                .AddExtension(new RepositoryModuleExtension())
                .AddExtension(new ServiceModuleExtension());
        }
    }
}