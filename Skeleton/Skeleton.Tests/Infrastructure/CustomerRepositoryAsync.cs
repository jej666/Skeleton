using System;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomerRepositoryAsync : RepositoryAsync<Customer, int>
    {
        private static readonly Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> Configurator =
            config => config.UsingConfigConnectionString("Default")
                .UsingAdvancedSettings()
                .SetCommandTimeout(30)
                .SetRetryPolicyCount(3)
                .SetRetryPolicyInterval(1);

        public CustomerRepositoryAsync(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseFactory databaseFactory)
            : base(typeAccessorCache, databaseFactory, Configurator)
        {
        }
    }
}