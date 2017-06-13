namespace Skeleton.Abstraction.Dependency
{
    public interface IBootstrapOrm : IHideObjectMethods
    {
        IBootstrapper WithOrm();
    }
}