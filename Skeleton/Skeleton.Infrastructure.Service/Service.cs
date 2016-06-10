using Skeleton.Abstraction;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using System;

namespace Skeleton.Infrastructure.Service
{
    public class Service<TEntity, TIdentity> :
        EntityService<TEntity, TIdentity>,
        IService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IRepository<TEntity, TIdentity> _repository;

        public Service(
            ILogger logger,
            IRepository<TEntity, TIdentity> repository)
            : base(logger)
        {
            repository.ThrowIfNull(() => repository);

            _repository = repository;
        }

        public IRepository<TEntity, TIdentity> Repository
        {
            get { return _repository; }
        }

        protected override void DisposeManagedResources()
        {
            _repository.Dispose();
        }
    }
}