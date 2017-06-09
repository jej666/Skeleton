namespace Skeleton.Abstraction.Dependency
{
    public interface IDependencyResolver : IHideObjectMethods
    {
        TService Resolve<TService>() where TService : class;
    }
}