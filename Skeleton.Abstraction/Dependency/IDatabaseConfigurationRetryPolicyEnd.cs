namespace Skeleton.Abstraction.Dependency
{
    public interface IDatabaseConfigurationRetryPolicyEnd : IHideObjectMethods
    {
        IDatabaseConfiguration SetRetryPolicyInterval(int value);
    }
}