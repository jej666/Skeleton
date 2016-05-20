using System;
using Microsoft.Practices.Unity;
using Skeleton.Common;

namespace Skeleton.Infrastructure.DependencyResolver
{
    public static class Bootstrapper
    {
        private static readonly Lazy<IUnityContainer> UnityContainer =
            new Lazy<IUnityContainer>(() => new UnityContainer());

        private static readonly Lazy<IContainer> Wrapper =
            new Lazy<IContainer>(() => new Container(UnityContainer.Value));

        public static IContainer Container
        {
            get { return Wrapper.Value; }
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