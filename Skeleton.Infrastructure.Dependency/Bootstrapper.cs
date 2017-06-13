using Skeleton.Abstraction.Dependency;
using Skeleton.Common;
using Skeleton.Infrastructure.Dependency.Plugins;
using System;

namespace Skeleton.Infrastructure.Dependency
{
    public class Bootstrapper : HideObjectMethodsBase, IBootstrapper, IBootstrapOrm
    {
        private readonly IDependencyContainer _container;

        public Bootstrapper() : this(DependencyContainer.Instance)
        {
        }

        public Bootstrapper(IDependencyContainer container)
        {
            container.ThrowIfNull(nameof(container));

            _container = container;
            _container.AddPlugin(new CorePlugin());
        }

        public IDependencyContainer Container => _container;

        public IBootstrapOrm UseSqlServer(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            configurator.ThrowIfNull(nameof(configurator));

            var builder = _container.Resolve<IDatabaseConfigurationBuilder>();
            _container.Register.Instance(configurator(builder));
            _container.AddPlugin(new DatabasePlugin());

            return this;
        }

        public IBootstrapper WithOrm()
        {
            _container.AddPlugin(new OrmPlugin());

            return this;
        }
    }
}