using System.Reflection;

namespace Skeleton.Abstraction
{
    public interface IMethodAccessor : IHideObjectMethods
    {
        MethodInfo MethodInfo { get; }

        string Name { get; }

        object Invoke(object instance, params object[] arguments);
    }
}