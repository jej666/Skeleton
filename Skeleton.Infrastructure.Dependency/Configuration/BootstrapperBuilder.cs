using Skeleton.Abstraction.Dependency;
using Skeleton.Common;
using Skeleton.Infrastructure.Dependency.Plugins;
using System;

namespace Skeleton.Infrastructure.Dependency.Configuration
{
    public class BootstrapperBuilder :
        HideObjectMethodsBase,
        IBootstrapperBuilder,
        IBootstrapOrm
    {
        private readonly IDependencyContainer _bootstrapper;

        public BootstrapperBuilder(IDependencyContainer bootstrapper)
        {
            bootstrapper.ThrowIfNull(nameof(bootstrapper));

            _bootstrapper = bootstrapper;
        }

        public IBootstrapOrm UseSqlServer(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            configurator.ThrowIfNull(nameof(configurator));

            var builder = _bootstrapper.Resolve<IDatabaseConfigurationBuilder>();
            _bootstrapper.Register.Instance(configurator(builder));
            _bootstrapper.AddPlugin(new DatabasePlugin());

            return this;
        }

        public IDependencyContainer WithOrm()
        {
            _bootstrapper.AddPlugin(new OrmPlugin());

            return _bootstrapper;
        }
    }
}