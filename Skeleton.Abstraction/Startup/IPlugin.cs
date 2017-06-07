namespace Skeleton.Abstraction.Startup
{
    public interface IPlugin
    {
        void Configure(IBootstrapper bootstrapper);
    }
}