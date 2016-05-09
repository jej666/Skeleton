namespace Skeleton.Infrastructure.Data.Configuration
{
    using Common;

    public interface IDatabaseConfigurationProperties : IHideObjectMethods
    {
        IDatabaseConfiguration Build();

        IDatabaseConfigurationSettings UsingAdvancedSettings();
    }
}