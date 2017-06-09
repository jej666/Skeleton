namespace Skeleton.Abstraction.Dependency
{
    public interface IBootstrapper : IHideObjectMethods
    {
        IBootstrapperBuilder Builder { get; }

        IDependencyContainer Container { get; }
    }
}