using Skeleton.Abstraction;

namespace Skeleton.Infrastructure.Data.Configuration
{
    public interface IDatabaseConfigurationSettings : IHideObjectMethods
    {
        IDatabaseConfigurationRetryPolicy SetCommandTimeout(int seconds);
    }
}