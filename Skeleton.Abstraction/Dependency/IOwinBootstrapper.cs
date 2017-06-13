namespace Skeleton.Abstraction.Dependency
{
    public interface IOwinBootstrapper: IBootstrapper
    {
        IOwinBootstrapper UseCheckModelForNull();
        IOwinBootstrapper UseGlobalExceptionHandling();
        IOwinBootstrapper UseSwagger();
        IOwinBootstrapper UseValidateModelState();
    }
}