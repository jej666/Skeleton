namespace Skeleton.Common.Reflection
{
    using System.Reflection;

    public interface IMethodAccessor
    {
        MethodInfo MethodInfo { get; }

        string Name { get; }

        object Invoke(object instance, params object[] arguments);
    }
}