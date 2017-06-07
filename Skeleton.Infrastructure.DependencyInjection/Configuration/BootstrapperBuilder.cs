using Skeleton.Abstraction.Startup;
using Skeleton.Common;
using Skeleton.Infrastructure.DependencyInjection.Plugins;
using System;

namespace Skeleton.Infrastructure.DependencyInjection.Configuration
{
    public class BootstrapperBuilder :
        HideObjectMethodsBase,
        IBootstrapperBuilder,
        IBootstrapOrm
    {
        private readonly IBootstrapper _bootstrapper;

        public BootstrapperBuilder(IBootstrapper bootstrapper)
        {
            bootstrapper.ThrowIfNull(nameof(bootstrapper));

            _bootstrapper = bootstrapper;
        }

        public IBootstrapOrm UseSqlServer(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            configurator.ThrowIfNull(nameof(configurator));

            var builder = _bootstrapper.Resolve<IDatabaseConfigurationBuilder>();
            _bootstrapper.RegisterInstance(configurator(builder));
            _bootstrapper.AddPlugin(new DatabasePlugin())
                         .AddPlugin(new AsyncDatabasePlugin());

            return this;
        }

        public IBootstrapper WithOrm()
        {
            _bootstrapper.AddPlugin(new OrmPlugin())
                         .AddPlugin(new AsyncOrmPlugin());

            return _bootstrapper;
        }
    }
}