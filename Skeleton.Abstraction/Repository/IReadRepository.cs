using System;

namespace Skeleton.Abstraction.Repository
{
    public interface IReadRepository<TEntity, TIdentity, TDto> :
            IDisposable,
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        IEntityReader<TEntity, TIdentity> Query { get; }

        IEntityMapper<TEntity, TIdentity, TDto> Mapper { get; }
    }
}