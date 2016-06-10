using Skeleton.Abstraction;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using System;

namespace Skeleton.Infrastructure.Service
{
    public class CachedService<TEntity, TIdentity> :
        EntityService<TEntity, TIdentity>,
        ICachedService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ICachedRepository<TEntity, TIdentity> _cachedRepository;

        public CachedService(
            ILogger logger,
            ICachedRepository<TEntity, TIdentity> cachedRepository)
            : base(logger)
        {
            cachedRepository.ThrowIfNull(() => cachedRepository);

            _cachedRepository = cachedRepository;
        }

        public ICachedRepository<TEntity, TIdentity> Repository
        {
            get { return _cachedRepository; }
        }

        protected override void DisposeManagedResources()
        {
            _cachedRepository.Dispose();
        }
    }
}