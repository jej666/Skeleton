using Skeleton.Abstraction;
using Skeleton.Abstraction.Dependency;
using Skeleton.Core;
using Skeleton.Infrastructure.Dependency.Configuration;
using Skeleton.Infrastructure.Logging;

namespace Skeleton.Infrastructure.Dependency.Plugins
{
    public sealed class LoggerPlugin : IPlugin
    {
        public void Configure(IDependencyContainer container)
        {
            container.ThrowIfNull(nameof(container));

            var configuration = new LoggerConfiguration();
            configuration.Configure();
            (container as DependencyContainer).UnityContainer.AddExtension(new LoggerConstructorInjectionExtension());

            container.Register.Instance<ILoggerFactory>(new LoggerFactory());
        }
    }
}