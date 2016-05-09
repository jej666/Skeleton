using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Infrastructure.Repository;
using System;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomerRepository : RepositoryBase<Customer, int>
    {
        private static readonly Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> Configurator =
            config => config.UsingConfigConnectionString("Default")
                            .UsingAdvancedSettings()
                            .SetCommandTimeout(30)
                            .SetRetryPolicyCount(3)
                            .SetRetryPolicyInterval(1);

        public CustomerRepository(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseFactory databaseFactory)
            : base(typeAccessorCache, databaseFactory, Configurator)
        { }

        public CustomerRepository(
            ITypeAccessorCache typeAccessorCache,
            IDatabase database)
            : base(typeAccessorCache, database)
        { }
    }
}