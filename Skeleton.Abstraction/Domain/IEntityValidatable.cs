namespace Skeleton.Abstraction.Domain
{
    public interface IEntityValidatable<out TEntity> : IHideObjectMethods
        where TEntity : class, IEntity<TEntity>, new()
    {
        IEntityValidationResult Validate(IEntityValidator<TEntity> validator);
    }
}