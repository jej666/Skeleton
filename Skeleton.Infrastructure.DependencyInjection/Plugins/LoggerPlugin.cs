using Skeleton.Abstraction;
using Skeleton.Abstraction.Startup;
using Skeleton.Infrastructure.Logging;

namespace Skeleton.Infrastructure.DependencyInjection.Plugins
{
    public sealed class LoggerPlugin : IPlugin
    {
        public void Configure(IBootstrapper bootstrapper)
        {
            var configuration = new LoggerConfiguration();
            configuration.Configure();

            bootstrapper.RegisterInstance<ILoggerFactory>(new LoggerFactory());
        }
    }
}