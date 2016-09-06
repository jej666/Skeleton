namespace Skeleton.Core.Reflection
{
    internal delegate object ConstructorDelegate(object[] parameters);

    internal delegate object GetterDelegate(object target);

    internal delegate object MethodDelegate(object target, object[] arguments);

    internal delegate void SetterDelegate(object target, object value);
}