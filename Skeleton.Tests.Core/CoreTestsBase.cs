using NUnit.Framework;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;
using Skeleton.Infrastructure.DependencyInjection;

namespace Skeleton.Tests.Core
{
    public abstract class CoreTestsBase
    {
        protected static IDependencyResolver Container = Bootstrapper.Resolver;
        
        public IMetadataProvider MetadataProvider => Container.Resolve<IMetadataProvider>();
    }
}