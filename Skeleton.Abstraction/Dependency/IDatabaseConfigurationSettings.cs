namespace Skeleton.Abstraction.Dependency
{
    public interface IDatabaseConfigurationSettings : IHideObjectMethods
    {
        IDatabaseConfigurationRetryPolicy SetCommandTimeout(int seconds);
    }
}