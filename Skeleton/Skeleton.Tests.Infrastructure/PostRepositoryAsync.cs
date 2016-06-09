using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Tests.Infrastructure
{
    public class PostRepositoryAsync : RepositoryAsync<Post, int>
    {
        public PostRepositoryAsync(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseAsync database)
            : base(typeAccessorCache, database)
        {
        }
    }
}