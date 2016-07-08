using System.Reflection;

namespace Skeleton.Shared.Abstraction.Reflection
{
    public interface IMethodAccessor : IHideObjectMethods
    {
        MethodInfo MethodInfo { get; }

        string Name { get; }

        object Invoke(object instance, params object[] arguments);
    }
}