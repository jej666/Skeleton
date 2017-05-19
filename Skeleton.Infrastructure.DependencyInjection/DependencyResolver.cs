using Microsoft.Practices.Unity;
using Skeleton.Abstraction;
using Skeleton.Common;

namespace Skeleton.Infrastructure.DependencyInjection
{
    public sealed class DependencyResolver :
        HideObjectMethodsBase,
        IDependencyResolver
    {
        private readonly IUnityContainer _unityContainer;

        public DependencyResolver(IUnityContainer container)
        {
            _unityContainer = container;
        }

        public TService Resolve<TService>() where TService : class
        {
            return _unityContainer.Resolve<TService>();
        }
    }
}