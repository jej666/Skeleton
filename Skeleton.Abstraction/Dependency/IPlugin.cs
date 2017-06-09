namespace Skeleton.Abstraction.Dependency
{ 
    public interface IPlugin
    {
        void Configure(IDependencyContainer container);
    }
}