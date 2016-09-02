using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public class CachedReadRepository<TEntity, TIdentity, TDto> :
            ReadRepository<TEntity, TIdentity, TDto>,
            ICachedReadRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public CachedReadRepository(
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