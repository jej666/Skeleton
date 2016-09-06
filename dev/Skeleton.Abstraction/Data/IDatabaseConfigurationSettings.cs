namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseConfigurationSettings : IHideObjectMethods
    {
        IDatabaseConfigurationRetryPolicy SetCommandTimeout(int seconds);
    }
}