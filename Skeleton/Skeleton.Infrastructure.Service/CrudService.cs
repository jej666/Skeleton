using System;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Service
{
    public class CrudService<TEntity, TIdentity> :
        EntityService<TEntity, TIdentity>,
        ICrudService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ICrudRepository<TEntity, TIdentity> _crudRepository;

        public CrudService(
            ILogger logger,
            ICrudRepository<TEntity, TIdentity> crudRepository)
            : base(logger)
        {
            crudRepository.ThrowIfNull(() => crudRepository);

            _crudRepository = crudRepository;
        }

        public ICrudRepository<TEntity, TIdentity> Repository
        {
            get { return _crudRepository; }
        }

        protected override void DisposeManagedResources()
        {
            _crudRepository.Dispose();
        }
    }
}