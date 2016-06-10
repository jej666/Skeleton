using Skeleton.Abstraction;

namespace Skeleton.Infrastructure.Data.Configuration
{
    public interface IDatabaseConfigurationBuilder : IHideObjectMethods
    {
        IDatabaseConfigurationProperties UsingConfigConnectionString(string connectionStringConfigName);

        IDatabaseConfigurationProperties UsingConnectionString(string connectionString);

        IDatabaseConfigurationProperties UsingDefaultConfigConnectionString();
    }
}