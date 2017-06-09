namespace Skeleton.Abstraction.Dependency
{
    public interface IDatabaseConfiguration : IHideObjectMethods
    {
        int CommandTimeout { get; set; }
        string ConnectionString { get; }
        string Name { get; }
        string ProviderName { get; }
        int RetryCount { get; set; }
        int RetryInterval { get; set; }
    }
}