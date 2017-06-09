namespace Skeleton.Abstraction.Dependency
{
    public interface IDatabaseConfigurationProperties : IHideObjectMethods
    {
        IDatabaseConfiguration Build();

        IDatabaseConfigurationSettings UsingAdvancedSettings();
    }
}