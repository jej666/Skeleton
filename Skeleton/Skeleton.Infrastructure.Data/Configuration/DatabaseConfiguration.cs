using Skeleton.Common;

namespace Skeleton.Infrastructure.Data.Configuration
{
    public sealed class DatabaseConfiguration :
        HideObjectMethods,
        IDatabaseConfiguration
    {
        private const int DefaultRetryPolicyCount = 5;
        private const int DefaultRetryPolicyInterval = 1;
        private const int DefaultTimeout = 300;

        private int _commandTimeout;
        private int _retryPolicyCount;
        private int _retryPolicyInterval;

        public int CommandTimeout
        {
            get { return _commandTimeout; }
            set { _commandTimeout = value == 0 ? DefaultTimeout : value; }
        }

        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool ProfilerActivated { get; set; }

        public int RetryPolicyCount
        {
            get { return _retryPolicyCount; }
            set { _retryPolicyCount = value == 0 ? DefaultRetryPolicyCount : value; }
        }

        public int RetryPolicyInterval
        {
            get { return _retryPolicyInterval; }
            set { _retryPolicyInterval = value == 0 ? DefaultRetryPolicyInterval : value; }
        }
    }
}