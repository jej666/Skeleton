using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Service
{
    public class CrudService<TEntity, TIdentity, TDto> :
        ReadService<TEntity, TIdentity, TDto>,
        ICrudService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly IEntityPersitor<TEntity, TIdentity> _persistor;

        public CrudService(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IEntityReader<TEntity, TIdentity> reader,
            IEntityPersitor<TEntity, TIdentity> persistor)
            : base(logger, mapper, reader)
        {
            _persistor = persistor;
        }

        public IEntityPersitor<TEntity, TIdentity> Store
        {
            get { return _persistor; }
        }

        protected override void DisposeManagedResources()
        {
            _persistor.Dispose();
        }
    }
}