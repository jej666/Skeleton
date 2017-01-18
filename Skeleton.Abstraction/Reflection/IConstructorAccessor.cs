namespace Skeleton.Abstraction.Reflection
{
    public interface IConstructorAccessor
    {
        object Invoke(params object[] arguments);
    }
}