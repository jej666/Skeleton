namespace Skeleton.Abstraction.Startup
{
    public interface IBootstrapOrm : IHideObjectMethods
    {
        IBootstrapper WithOrm();
    }
}