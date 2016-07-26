using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Data.Configuration
{
    public interface IDatabaseConfigurationRetryPolicyEnd : IHideObjectMethods
    {
        IDatabaseConfiguration SetRetryPolicyInterval(int value);
    }
}