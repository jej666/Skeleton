namespace Skeleton.Infrastructure.Data.Configuration
{
    using Common;

    public interface IDatabaseConfigurationBuilder : IHideObjectMethods
    {
        IDatabaseConfigurationProperties UsingConfigConnectionString(string connectionStringConfigName);

        IDatabaseConfigurationProperties UsingConnectionString(string connectionString);

        IDatabaseConfigurationProperties UsingDefaultConfigConnectionString();
    }
}