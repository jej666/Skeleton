using System;

namespace Skeleton.Abstraction.Dependency
{
    public interface IBootstrapperBuilder : IHideObjectMethods
    {
        IBootstrapOrm UseSqlServer(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator);
    }
}