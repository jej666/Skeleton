using Skeleton.Abstraction.Data;
using Skeleton.Common;
using System.Diagnostics;

namespace Skeleton.Infrastructure.Data
{
    [DebuggerDisplay("DatabaseName = {Name")]
    public sealed class DatabaseConfiguration :
        HideObjectMethodsBase,
        IDatabaseConfiguration
    {
        public static readonly int DefaultTimeout = 300;
        public static readonly int DefaultRetryCount = 10;
        public static readonly int DefaultRetryInterval = 1;

        private int _commandTimeout;
        private int _retryCount;
        private int _retryInterval;

        public int CommandTimeout
        {
            get { return _commandTimeout; }
            set { _commandTimeout = value == 0 ? DefaultTimeout : value; }
        }

        public string ConnectionString { get; set; }

        public string Name { get; set; }

        public string ProviderName { get; set; }

        public int RetryCount
        {
            get { return _retryCount; }
            set { _retryCount = value == 0 ? DefaultRetryCount : value; }
        }

        public int RetryInterval
        {
            get { return _retryInterval; }
            set { _retryInterval = value == 0 ? DefaultRetryInterval : value; }
        }
    }
}