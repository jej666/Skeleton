using Skeleton.Abstraction;

namespace Skeleton.Infrastructure.Data
{
    internal static class DataReaderMapperFactory
    {
        internal static DataReaderMapper<TResult> CreateMapper<TResult>(
            this ITypeAccessorCache accessorCache)
            where TResult : class
        {
            return new DataReaderMapper<TResult>(accessorCache);
        }

        internal static DataReaderMapperAsync<TResult> CreateMapperAsync<TResult>(
            this ITypeAccessorCache accessorCache)
            where TResult : class
        {
            return new DataReaderMapperAsync<TResult>(accessorCache);
        }
    }
}