namespace Skeleton.Core
{
    public interface IInstanceCreator
    {
        object CreateInstance();
        object CreateInstance(object[] parameters);
        T CreateInstance<T>();
        T CreateInstance<T>(object[] parameters);
    }
}