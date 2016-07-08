namespace Skeleton.Abstraction.Reflection
{
    public interface IMemberAccessor : IMemberInfo
    {
        object GetValue(object instance);

        void SetValue(object instance, object value);
    }
}