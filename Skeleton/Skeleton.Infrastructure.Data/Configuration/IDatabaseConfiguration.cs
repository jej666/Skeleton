using Skeleton.Common;

namespace Skeleton.Infrastructure.Data.Configuration
{
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