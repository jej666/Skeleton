using Skeleton.Core;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncCachedReadService<TEntity, TIdentity, TDto> :
            AsyncReadService<TEntity, TIdentity, TDto>,
            IAsyncCachedReadService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public AsyncCachedReadService(
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