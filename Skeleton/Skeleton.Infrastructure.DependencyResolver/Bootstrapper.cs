namespace Skeleton.Infrastructure.DependencyResolver
{
    using Common;
    using Microsoft.Practices.Unity;
    using System;

    public static class Bootstrapper
    {
        private readonly static Lazy<IUnityContainer> unityContainer =
            new Lazy<IUnityContainer>(() => new UnityContainer());

        private readonly static Lazy<IContainer> wrapper =
            new Lazy<IContainer>(() => new Container(unityContainer.Value));

        public static IContainer Container { get { return wrapper.Value; } }

        public static void Initialize()
        {
            unityContainer.Value.RegisterInstance<IContainer>(Container);

            unityContainer.Value
                .AddExtension(new CommonModuleExtension())
                .AddExtension(new DataModuleExtension())
                .AddExtension(new RepositoryModuleExtension())
                .AddExtension(new ServiceModuleExtension());
        }
    }
}