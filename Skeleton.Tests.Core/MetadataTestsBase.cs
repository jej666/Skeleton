using Skeleton.Abstraction.Reflection;
using Skeleton.Infrastructure.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Tests.Core
{
    public abstract class MetadataTestsBase
    {
        protected MetadataTestsBase()
        {
            MetadataProvider = Bootstrapper.Resolver.Resolve<IMetadataProvider>();
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