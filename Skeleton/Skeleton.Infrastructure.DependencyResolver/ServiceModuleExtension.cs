using Microsoft.Practices.Unity;
using Skeleton.Core.Service;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class ServiceModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType(typeof(IAggregateService), typeof(Service.Service));
        }
    }
}