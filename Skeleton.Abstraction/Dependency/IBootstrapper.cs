using System;

namespace Skeleton.Abstraction.Dependency
{
    public interface IBootstrapper : IHideObjectMethods
    {
        IDependencyContainer Container { get; }

        IBootstrapOrm UseSqlServer(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator);
    }
}