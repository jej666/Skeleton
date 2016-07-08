using System;

namespace Skeleton.Abstraction.Reflection
{
    public interface IMethodMetadata
    {
        int MethodsCount { get; }
        IMethodAccessor GetMethod(string name, params object[] parameters);

        IMethodAccessor GetMethod(string name, Type[] parameterTypes);
    }
}