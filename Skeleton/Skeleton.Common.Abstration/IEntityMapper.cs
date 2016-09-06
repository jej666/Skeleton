using Skeleton.Abstraction.Reflection;

namespace Skeleton.Abstraction
{
    public interface IEntityMapper<TEntity, TIdentity> : IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IMetadata TypeAccessor { get; }

        TDto Map<TDto>(TEntity entity) where TDto : class;

        TEntity Reverse<TDto>(TDto dto) where TDto : class;

        TEntity Reverse<TDto>(int id, TDto dto) where TDto : class;
    }
}
