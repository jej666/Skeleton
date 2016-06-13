
namespace Skeleton.Abstraction
{
    public interface IEntityMapper<TEntity, TIdentity>
            where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ITypeAccessor TypeAccessor {get;}

        Dto Map<Dto>(TEntity entity) where Dto : class;
    }
}
