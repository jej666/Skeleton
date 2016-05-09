namespace Skeleton.Infrastructure.Data.Configuration
{
    using Common;

    public interface IDatabaseConfigurationSettings : IHideObjectMethods
    {
        IDatabaseConfigurationRetryPolicy SetCommandTimeout(int seconds);
    }
}