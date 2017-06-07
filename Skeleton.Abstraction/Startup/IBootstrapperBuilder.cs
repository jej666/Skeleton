using System;

namespace Skeleton.Abstraction.Startup
{
    public interface IBootstrapperBuilder : IHideObjectMethods
    {
        IBootstrapOrm UseSqlServer(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator);
    }
}