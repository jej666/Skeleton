namespace Skeleton.Infrastructure.DependencyResolver.LoggerExtension
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class LoggerConstructorInjectionExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<CreationStackTrackerStrategy>(UnityBuildStage.TypeMapping);
            Context.Strategies.AddNew<LogBuilderStrategy>(UnityBuildStage.TypeMapping);
        }
    }
}