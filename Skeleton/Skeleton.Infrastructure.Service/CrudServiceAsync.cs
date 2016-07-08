using System;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Service
{
    public class CrudServiceAsync<TEntity, TIdentity> :
        EntityService<TEntity, TIdentity>,
        ICrudServiceAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ICrudRepositoryAsync<TEntity, TIdentity> _repositoryAsync;

        public CrudServiceAsync(
            ILogger logger,
            ICrudRepositoryAsync<TEntity, TIdentity> repositoryAsync)
            : base(logger)
        {
            repositoryAsync.ThrowIfNull(() => repositoryAsync);

            _repositoryAsync = repositoryAsync;
        }

        public ICrudRepositoryAsync<TEntity, TIdentity> Repository
        {
            get { return _repositoryAsync; }
        }

        protected override void DisposeManagedResources()
        {
            _repositoryAsync.Dispose();
        }
    }
}