using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Dependency;
using Skeleton.Core;
using Skeleton.Infrastructure.Data;

namespace Skeleton.Infrastructure.Dependency.Plugins
{
    public sealed class DatabasePlugin : IPlugin
    {
        public void Configure(IDependencyContainer container)
        {
            container.ThrowIfNull(nameof(container));

            container.Register
                .Type<IDatabase, Database>(DependencyLifetime.Scoped)
                .Type<IAsyncDatabase, AsyncDatabase>(DependencyLifetime.Scoped);
        }
    }
}