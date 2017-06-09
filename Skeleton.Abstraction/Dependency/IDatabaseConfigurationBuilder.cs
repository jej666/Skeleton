namespace Skeleton.Abstraction.Dependency
{
    public interface IDatabaseConfigurationBuilder : IHideObjectMethods
    {
        IDatabaseConfigurationProperties UsingConfigConnectionString(string connectionStringConfigName);

        IDatabaseConfigurationProperties UsingConnectionString(string connectionString);

        IDatabaseConfigurationProperties UsingDefaultConfigConnectionString();
    }
}