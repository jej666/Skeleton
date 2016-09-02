using Skeleton.Core;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public class CachedReadService<TEntity, TIdentity, TDto> :
            ReadService<TEntity, TIdentity, TDto>,
            ICachedReadService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public CachedReadService(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            ICachedEntityReader<TEntity, TIdentity> reader)
            : base(logger, mapper, reader)
        {
            Query = reader;
        }

        public new ICachedEntityReader<TEntity, TIdentity> Query { get; }

        protected override void DisposeManagedResources()
        {
            Query.Dispose();
        }
    }
}