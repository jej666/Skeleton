using System;
using System.Collections.Generic;
using System.Linq;
using Skeleton.Abstraction.Reflection;
using Skeleton.Infrastructure.DependencyInjection;

namespace Skeleton.Tests.Core
{
    public abstract class MetadataTestsBase
    {
        protected MetadataTestsBase()
        {
            Bootstrapper.Initialize();
            var container = Bootstrapper.Resolver;
            MetadataProvider = container.Resolve<IMetadataProvider>();
        }

        public static IMetadataProvider MetadataProvider { get; private set; }

        public static IEnumerable<Type> MatchingInCache(IMetadata metadata)
        {
            var matchingInCache = MetadataProvider
                .Types
                .Where(t => t == metadata.Type);

            return matchingInCache.ToList();
        }
    }
}