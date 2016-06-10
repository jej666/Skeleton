namespace Skeleton.Abstraction
{
    public interface IValidatable<out TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IValidationResult Validate(IValidator<TEntity, TIdentity> validator);
    }
}