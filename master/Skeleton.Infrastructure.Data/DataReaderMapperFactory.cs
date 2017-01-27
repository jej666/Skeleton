using Skeleton.Abstraction.Reflection;
using System.Data;

namespace Skeleton.Infrastructure.Data
{
    internal static class DataReaderMapperFactory
    {
        internal static DataReaderMapper<TResult> CreateMapper<TResult>(
            this IMetadataProvider accessorCache, IDataReader dataReader)
            where TResult : class
        {
            return new DataReaderMapper<TResult>(accessorCache, dataReader);
        }

        internal static AsyncDataReaderMapper<TResult> CreateMapperAsync<TResult>(
            this IMetadataProvider accessorCache, IDataReader dataReader)
            where TResult : class
        {
            return new AsyncDataReaderMapper<TResult>(accessorCache, dataReader);
        }
    }
}