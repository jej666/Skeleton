namespace Skeleton.Abstraction
{
    public interface IEntityValidatable<out TEntity, TIdentity> : IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IEntityValidationResult Validate(IEntityValidator<TEntity, TIdentity> validator);
    }
}