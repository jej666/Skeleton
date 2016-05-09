namespace Skeleton.Infrastructure.Data.Configuration
{
    using Common;
    using Common.Reflection;

    public interface IDatabaseConfiguration : IHideObjectMethods
    {
        int CommandTimeout { get; set; }
        string ConnectionString { get; }
        string Name { get; }
        string ProviderName { get; }
        int RetryPolicyCount { get; set; }
        int RetryPolicyInterval { get; set; }
        bool ProfilerActivated { get; set; }
    }
}