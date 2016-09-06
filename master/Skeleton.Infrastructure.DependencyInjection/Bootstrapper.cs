﻿using System;
using Microsoft.Practices.Unity;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Common;

namespace Skeleton.Infrastructure.DependencyInjection
{
    public static class Bootstrapper
    {
        private static readonly Lazy<IUnityContainer> UnityContainer =
            new Lazy<IUnityContainer>(() => new UnityContainer());

        private static readonly Lazy<IDependencyResolver> ContainerWrapper =
            new Lazy<IDependencyResolver>(() => new DependencyResolver(UnityContainer.Value));

        private static readonly Lazy<IDependencyRegistrar> RegistrarWrapper =
            new Lazy<IDependencyRegistrar>(() => new DependencyRegistrar(UnityContainer.Value));

        public static IDependencyResolver Resolver => ContainerWrapper.Value;

        public static IDependencyRegistrar Registrar => RegistrarWrapper.Value;

        public static IUnityContainer Container => UnityContainer.Value;

        public static void Initialize()
        {
            UnityContainer.Value.RegisterInstance(Resolver);

            UnityContainer.Value
                .AddExtension(new CommonModuleExtension())
                .AddExtension(new DataModuleExtension())
                .AddExtension(new RepositoryModuleExtension())
                .AddExtension(new AsyncRepositoryModuleExtension());
        }

        public static void UseDatabase(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            configurator.ThrowIfNull(() => configurator);

            var builder = Resolver.Resolve<IDatabaseConfigurationBuilder>();
            UnityContainer.Value.RegisterInstance(configurator.Invoke(builder));
        }
    }
}