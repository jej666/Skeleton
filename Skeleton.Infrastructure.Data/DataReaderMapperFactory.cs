using Skeleton.Abstraction.Reflection;
using System.Data;

namespace Skeleton.Infrastructure.Data
{
    internal static class DataReaderMapperFactory
    {
        internal static DataReaderMapper<TResult> CreateMapper<TResult>(
            this IMetadataProvider metadataProvider,
            IDataReader dataReader)
            where TResult : class
        {
            return new DataReaderMapper<TResult>(metadataProvider, dataReader);
        }

        internal static AsyncDataReaderMapper<TResult> CreateMapperAsync<TResult>(
            this IMetadataProvider metadataProvider,
            IDataReader dataReader)
            where TResult : class
        {
            return new AsyncDataReaderMapper<TResult>(metadataProvider, dataReader);
        }
    }
}