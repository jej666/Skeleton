using System.Collections.Generic;
using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface IQuery<TEntity, in TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        TEntity FirstOrDefault();

        TEntity FirstOrDefault(TIdentity id);

        IEnumerable<TEntity> Find();

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Page(int pageSize, int pageNumber);
    }
}