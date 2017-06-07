namespace Skeleton.Abstraction.Startup
{
    public interface IDatabaseConfigurationRetryPolicy : IHideObjectMethods
    {
        IDatabaseConfiguration Build();

        IDatabaseConfigurationRetryPolicyEnd SetRetryPolicyCount(int value);
    }
}