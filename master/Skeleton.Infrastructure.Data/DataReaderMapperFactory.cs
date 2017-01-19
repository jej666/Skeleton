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

        internal static DataReaderMapperAsync<TResult> CreateMapperAsync<TResult>(
            this IMetadataProvider accessorCache, IDataReader dataReader)
            where TResult : class
        {
            return new DataReaderMapperAsync<TResult>(accessorCache, dataReader);
        }
    }
}