using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Data
{
    internal static class DataReaderMapperFactory
    {
        internal static DataReaderMapper<TResult> CreateMapper<TResult>(
            this IMetadataProvider accessorCache)
            where TResult : class
        {
            return new DataReaderMapper<TResult>(accessorCache);
        }

        internal static DataReaderMapperAsync<TResult> CreateMapperAsync<TResult>(
            this IMetadataProvider accessorCache)
            where TResult : class
        {
            return new DataReaderMapperAsync<TResult>(accessorCache);
        }
    }
}