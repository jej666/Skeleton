using System;

namespace Skeleton.Abstraction.Reflection
{
    public interface IMethodMetadata
    {
        IMethodAccessor GetMethod(string name);

        IMethodAccessor GetMethod(string name, Type[] parameterTypes);
    }
}