namespace Skeleton.Abstraction
{
    public interface IDependencyResolver : IHideObjectMethods
    {
        TService Resolve<TService>() where TService : class;
    }
}