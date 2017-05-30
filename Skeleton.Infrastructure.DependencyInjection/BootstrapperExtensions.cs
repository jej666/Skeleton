using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Common;
using System;

namespace Skeleton.Infrastructure.DependencyInjection
{
    public static class BootstrapperExtensions
    {
        public static IBootstrapper UseDatabase(this IBootstrapper bootstrapper, Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            bootstrapper.ThrowIfNull(nameof(bootstrapper));
            configurator.ThrowIfNull(nameof(configurator));

            var builder = bootstrapper.Resolve<IDatabaseConfigurationBuilder>();
            bootstrapper.RegisterInstance(configurator.Invoke(builder));

            return bootstrapper;
        }

        public static IBootstrapper UseOrm(this IBootstrapper bootstrapper)
        {
            bootstrapper.ThrowIfNull(nameof(bootstrapper));
           
            bootstrapper.AddPlugin(new OrmPlugin());

            return bootstrapper;
        }

        public static IBootstrapper UseAsyncOrm(this IBootstrapper bootstrapper)
        {
            bootstrapper.ThrowIfNull(nameof(bootstrapper));

            bootstrapper.AddPlugin(new AsyncOrmPlugin());

            return bootstrapper;
        }
    }
}