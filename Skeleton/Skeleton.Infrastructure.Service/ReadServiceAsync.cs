using System;
using Skeleton.Abstraction;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;

namespace Skeleton.Infrastructure.Service
{
    public class ReadServiceAsync<TEntity, TIdentity> :
        EntityService<TEntity, TIdentity>,
        IReadServiceAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IReadRepositoryAsync<TEntity, TIdentity> _readRepositoryAsync;

        public ReadServiceAsync(
            ILogger logger,
            IReadRepositoryAsync<TEntity, TIdentity> readRepositoryAsync)
            : base(logger)
        {
            readRepositoryAsync.ThrowIfNull(() => readRepositoryAsync);

            _readRepositoryAsync = readRepositoryAsync;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> Repository
        {
            get { return _readRepositoryAsync; }
        }

        protected override void DisposeManagedResources()
        {
            _readRepositoryAsync.Dispose();
        }
    }
}