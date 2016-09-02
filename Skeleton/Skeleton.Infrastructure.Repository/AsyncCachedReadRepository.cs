using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncCachedReadRepository<TEntity, TIdentity, TDto> :
            AsyncReadRepository<TEntity, TIdentity, TDto>,
            IAsyncCachedReadRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public AsyncCachedReadRepository(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IAsyncCachedEntityReader<TEntity, TIdentity> reader)
            : base(logger, mapper, reader)
        {
            Query = reader;
        }

        public new IAsyncCachedEntityReader<TEntity, TIdentity> Query { get; }

        protected override void DisposeManagedResources()
        {
            Query.Dispose();
        }
    }
}