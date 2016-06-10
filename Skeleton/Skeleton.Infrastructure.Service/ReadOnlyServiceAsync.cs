using Skeleton.Abstraction;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using System;

namespace Skeleton.Infrastructure.Service
{
    public class ReadOnlyServiceAsync<TEntity, TIdentity> :
        EntityService<TEntity, TIdentity>,
        IReadOnlyServiceAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IReadOnlyRepositoryAsync<TEntity, TIdentity> _readOnlyRepositoryAsync;

        public ReadOnlyServiceAsync(
            ILogger logger,
            IReadOnlyRepositoryAsync<TEntity, TIdentity> readOnlyRepositoryAsync)
            : base(logger)
        {
            readOnlyRepositoryAsync.ThrowIfNull(() => readOnlyRepositoryAsync);

            _readOnlyRepositoryAsync = readOnlyRepositoryAsync;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> Repository
        {
            get { return _readOnlyRepositoryAsync; }
        }

        protected override void DisposeManagedResources()
        {
            _readOnlyRepositoryAsync.Dispose();
        }
    }
}