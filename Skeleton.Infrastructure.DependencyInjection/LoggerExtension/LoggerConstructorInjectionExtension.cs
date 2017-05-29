using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Skeleton.Infrastructure.Logging;

namespace Skeleton.Infrastructure.DependencyInjection.LoggerExtension
{
    public sealed class LoggerConstructorInjectionExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            LoggerConfiguration.Configure();

            Context.Strategies.AddNew<CreationStackTrackerStrategy>(UnityBuildStage.TypeMapping);
            Context.Strategies.AddNew<LogBuilderStrategy>(UnityBuildStage.TypeMapping);
        }
    }
}