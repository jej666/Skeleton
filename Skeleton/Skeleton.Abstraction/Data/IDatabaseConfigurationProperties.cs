namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseConfigurationProperties : IHideObjectMethods
    {
        IDatabaseConfiguration Build();

        IDatabaseConfigurationSettings UsingAdvancedSettings();
    }
}