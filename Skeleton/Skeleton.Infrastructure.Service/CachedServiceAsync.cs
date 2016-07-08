using System;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Service
{
    public class CachedServiceAsync<TEntity, TIdentity> :
        EntityService<TEntity, TIdentity>,
        ICachedServiceAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ICachedRepositoryAsync<TEntity, TIdentity> _cachedRepositoryAsync;

        public CachedServiceAsync(
            ILogger logger,
            ICachedRepositoryAsync<TEntity, TIdentity> cachedRepositoryAsync)
            : base(logger)
        {
            cachedRepositoryAsync.ThrowIfNull(() => cachedRepositoryAsync);

            _cachedRepositoryAsync = cachedRepositoryAsync;
        }

        public ICachedRepositoryAsync<TEntity, TIdentity> Repository
        {
            get { return _cachedRepositoryAsync; }
        }

        protected override void DisposeManagedResources()
        {
            _cachedRepositoryAsync.Dispose();
        }
    }
}