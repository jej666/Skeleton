using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Common;
using System;

namespace Skeleton.Infrastructure.DependencyInjection
{
    public static class AppHostExtensions
    {
        public static IAppHost UseDatabase(this IAppHost appHost, Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            appHost.ThrowIfNull(nameof(appHost));
            configurator.ThrowIfNull(nameof(configurator));

            var builder = appHost.Resolve<IDatabaseConfigurationBuilder>();
            appHost.RegisterInstance(configurator.Invoke(builder));

            return appHost;
        }

        public static IAppHost UseOrm(this IAppHost appHost)
        {
            appHost.ThrowIfNull(nameof(appHost));
           
            appHost.AddPlugin(new OrmPlugin());

            return appHost;
        }

        public static IAppHost UseAsyncOrm(this IAppHost appHost)
        {
            appHost.ThrowIfNull(nameof(appHost));

            appHost.AddPlugin(new AsyncOrmPlugin());

            return appHost;
        }
    }
}
