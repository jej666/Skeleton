using Skeleton.Common;

namespace Skeleton.Infrastructure.Data.Configuration
{
    public interface IDatabaseConfigurationRetryPolicy : IHideObjectMethods
    {
        IDatabaseConfiguration Build();

        IDatabaseConfigurationRetryPolicyEnd SetRetryPolicyCount(int value);
    }
}