namespace Skeleton.Shared.Abstraction
{
    public interface IEntityValidatable<out TEntity, TIdentity> : IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IValidationResult Validate(IEntityValidator<TEntity, TIdentity> validator);
    }
}