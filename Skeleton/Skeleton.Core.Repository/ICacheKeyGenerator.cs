using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface ICacheKeyGenerator<TEntity, in TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        string ForFind(ISqlQuery query);
        string ForFirstOrDefault(TIdentity id);
        string ForFirstOrDefault(ISqlQuery query);
        string ForGetAll();
        string ForPage(int pageSize, int pageNumber);
    }
}