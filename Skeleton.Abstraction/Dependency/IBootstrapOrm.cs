namespace Skeleton.Abstraction.Dependency
{
    public interface IBootstrapOrm : IHideObjectMethods
    {
        IDependencyContainer WithOrm();
    }
}