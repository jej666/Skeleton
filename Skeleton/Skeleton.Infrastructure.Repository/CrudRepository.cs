using Skeleton.Core;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public class CrudRepository<TEntity, TIdentity, TDto> :
            ReadRepository<TEntity, TIdentity, TDto>,
            ICrudRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public CrudRepository(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IEntityReader<TEntity, TIdentity> reader,
            IEntityPersitor<TEntity, TIdentity> persistor)
            : base(logger, mapper, reader)
        {
            Store = persistor;
        }

        public IEntityPersitor<TEntity, TIdentity> Store { get; }

        protected override void DisposeManagedResources()
        {
            Store.Dispose();
        }
    }
}