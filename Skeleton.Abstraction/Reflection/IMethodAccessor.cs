using System;
using System.Reflection;

namespace Skeleton.Abstraction.Reflection
{
    public interface IMethodAccessor : IHideObjectMethods
    {
        MethodInfo MethodInfo { get; }

        string Name { get; }

        Func<object, object[], object> Invoker { get; }
    }
}