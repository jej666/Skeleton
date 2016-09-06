using System;

namespace Skeleton.Abstraction.Reflection
{
    public interface IMethodMetadata
    {
        IMethodAccessor GetMethod(string name, params object[] parameters);

        IMethodAccessor GetMethod(string name, Type[] parameterTypes);

        int MethodsCount { get; }
    }
}
