using System;

namespace Skeleton.Abstraction.Reflection
{
    public interface IInstanceMetadata
    {
        IInstanceAccessor GetConstructor();

        IInstanceAccessor GetConstructor(Type[] parameterTypes);
    }
}