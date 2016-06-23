using Skeleton.Abstraction.Reflection;
using System;

namespace Skeleton.Common.Reflection
{
    public static class MetadataProviderExtension
    {
        public static IMetadata GetMetadata(this Type type)
        {
            return new MetadataProvider().GetMetadata(type);
        }
    }
}
