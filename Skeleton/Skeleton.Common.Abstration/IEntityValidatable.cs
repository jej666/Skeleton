namespace Skeleton.Abstraction
{
    public interface IEntityValidatable<TEntity, TIdentity> : IHideObjectMethods
          where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IValidationResult Validate(IEntityValidator<TEntity, TIdentity> validator);
    }
}