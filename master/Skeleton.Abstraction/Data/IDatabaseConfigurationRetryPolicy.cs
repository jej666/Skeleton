namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseConfigurationRetryPolicy : IHideObjectMethods
    {
        IDatabaseConfiguration Build();

        IDatabaseConfigurationRetryPolicyEnd SetRetryPolicyCount(int value);
    }
}