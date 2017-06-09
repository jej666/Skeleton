using Skeleton.Abstraction.Dependency;
using Skeleton.Common;
using Skeleton.Infrastructure.Dependency.Configuration;

namespace Skeleton.Infrastructure.Dependency
{
    public class Bootstrapper : HideObjectMethodsBase, IBootstrapper
    {
        private readonly IDependencyContainer _container;

        public Bootstrapper() : this(DependencyContainer.Instance)
        {
        }

        public Bootstrapper(IDependencyContainer container)
        {
            container.ThrowIfNull(nameof(container));

            _container = container;
        }

        public IBootstrapperBuilder Builder => new BootstrapperBuilder(Container);

        public IDependencyContainer Container => _container;
    }
}