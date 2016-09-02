using Skeleton.Core;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public class CrudService<TEntity, TIdentity, TDto> :
            ReadService<TEntity, TIdentity, TDto>,
            ICrudService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public CrudService(
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