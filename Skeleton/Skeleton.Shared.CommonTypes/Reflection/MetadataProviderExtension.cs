using System;
using Skeleton.Shared.Abstraction.Reflection;

namespace Skeleton.Shared.CommonTypes.Reflection
{
    public static class MetadataProviderExtension
    {
        public static IMetadata GetMetadata(this Type type)
        {
            return new MetadataProvider().GetMetadata(type);
        }
    }
}