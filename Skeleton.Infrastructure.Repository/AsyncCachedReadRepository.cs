using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class AsyncCachedReadRepository<TEntity, TDto> :
            AsyncReadRepository<TEntity, TDto>,
            IAsyncCachedReadRepository<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        public AsyncCachedReadRepository(
            IEntityMapper<TEntity, TDto> mapper,
            IAsyncCachedEntityReader<TEntity> reader)
            : base(mapper, reader)
        {
            Query = reader;
        }

        public new IAsyncCachedEntityReader<TEntity> Query { get; }
    }
}