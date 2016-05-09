namespace Skeleton.Infrastructure.Data.Configuration
{
    using Common;

    public interface IDatabaseConfigurationRetryPolicy : IHideObjectMethods
    {
        IDatabaseConfiguration Build();

        IDatabaseConfigurationRetryPolicyEnd SetRetryPolicyCount(int value);
    }
}