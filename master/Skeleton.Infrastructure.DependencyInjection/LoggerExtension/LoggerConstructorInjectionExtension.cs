using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Skeleton.Infrastructure.DependencyInjection.LoggerExtension
{
    public class LoggerConstructorInjectionExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<CreationStackTrackerStrategy>(UnityBuildStage.TypeMapping);
            Context.Strategies.AddNew<LogBuilderStrategy>(UnityBuildStage.TypeMapping);
        }
    }
}