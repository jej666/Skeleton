using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Core.Domain;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;

namespace Skeleton.Infrastructure.Service
{
    public class ServiceAsync<TEntity, TIdentity> :
        EntityService<TEntity, TIdentity>,
        IServiceAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IRepositoryAsync<TEntity, TIdentity> _repositoryAsync;

        public ServiceAsync(
            ILogger logger,
            IRepositoryAsync<TEntity, TIdentity> repositoryAsync) 
            : base(logger)
        {
            repositoryAsync.ThrowIfNull(() => repositoryAsync);

            _repositoryAsync = repositoryAsync;
        }

        public IRepositoryAsync<TEntity, TIdentity> Repository
        {
            get { return _repositoryAsync; }
        }

        protected override void DisposeManagedResources()
        {
            _repositoryAsync.Dispose();
        }
    }
}