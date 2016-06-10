using Skeleton.Abstraction;

namespace Skeleton.Infrastructure.Data.Configuration
{
    public interface IDatabaseConfigurationProperties : IHideObjectMethods
    {
        IDatabaseConfiguration Build();

        IDatabaseConfigurationSettings UsingAdvancedSettings();
    }
}