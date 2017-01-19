using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class CachedReadRepository<TEntity, TDto> :
            ReadRepository<TEntity, TDto>,
            ICachedReadRepository<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        public CachedReadRepository(
            IEntityMapper<TEntity, TDto> mapper,
            ICachedEntityReader<TEntity> reader)
            : base(mapper, reader)
        {
            Query = reader;
        }

        public new ICachedEntityReader<TEntity> Query { get; }
    }
}