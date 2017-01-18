namespace Skeleton.Abstraction.Reflection
{
    public interface IInstanceCreator
    {
        int ConstructorCount
        {
            get;
        }

        object CreateInstance();
        object CreateInstance(object[] parameters);
        T CreateInstance<T>();
        T CreateInstance<T>(object[] parameters);
    }
}