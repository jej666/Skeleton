namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseConfigurationBuilder : IHideObjectMethods
    {
        IDatabaseConfigurationProperties UsingConfigConnectionString(string connectionStringConfigName);

        IDatabaseConfigurationProperties UsingConnectionString(string connectionString);

        IDatabaseConfigurationProperties UsingDefaultConfigConnectionString();
    }
}