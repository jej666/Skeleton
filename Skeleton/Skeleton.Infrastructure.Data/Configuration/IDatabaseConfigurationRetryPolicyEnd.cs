namespace Skeleton.Infrastructure.Data.Configuration
{
    using Common;

    public interface IDatabaseConfigurationRetryPolicyEnd : IHideObjectMethods
    {
        IDatabaseConfiguration SetRetryPolicyInterval(int value);
    }
}