using Skeleton.Abstraction;
using Skeleton.Abstraction.Dependency;
using Skeleton.Infrastructure.Logging;

namespace Skeleton.Infrastructure.Dependency.Plugins
{
    public sealed class LoggerPlugin : IPlugin
    {
        public void Configure(IDependencyContainer container)
        {
            var configuration = new LoggerConfiguration();
            configuration.Configure();

            container.Register.Instance<ILoggerFactory>(new LoggerFactory());
        }
    }
}