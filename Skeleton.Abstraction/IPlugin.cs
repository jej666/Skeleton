namespace Skeleton.Abstraction
{
    public interface IPlugin
    {
        void Configure(IBootstrapper bootstrapper);
    }
}