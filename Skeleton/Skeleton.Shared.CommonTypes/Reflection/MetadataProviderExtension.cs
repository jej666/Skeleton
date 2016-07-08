using System;
using Skeleton.Abstraction.Reflection;

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