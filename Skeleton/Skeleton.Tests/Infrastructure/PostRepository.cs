using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Tests.Infrastructure
{
    public class PostRepository : RepositoryBase<Post, int>
    {
        public PostRepository(
            ITypeAccessorCache typeAccessorCache,
            IDatabase database)
            : base(typeAccessorCache, database)
        { }
    }
}