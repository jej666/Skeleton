using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Skeleton.Abstraction;

namespace Skeleton.Infrastructure.Data
{
    internal sealed class DataReaderMapperAsync<TResult> :
        DataReaderMapperBase<TResult>
        where TResult : class
    {
        internal DataReaderMapperAsync(ITypeAccessorCache accessorCache)
            : base(accessorCache)
        {
        }

        internal async Task<IEnumerable<TResult>> MapQueryAsync(DbDataReader dataReader)
        {
            try
            {
                var list = new List<TResult>();

                if (dataReader == null || dataReader.FieldCount == 0)
                    return list;

                while (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    var values = new object[dataReader.FieldCount];
                    dataReader.GetValues(values);

                    var instance = SetMatchingValues(dataReader, values);

                    list.Add(instance);
                }
                while (await dataReader.NextResultAsync().ConfigureAwait(false))
                {
                }

                return list;
            }
            finally
            {
                using (dataReader)
                {
                }
            }
        }

        internal async Task<TResult> MapSingleAsync(DbDataReader dataReader)
        {
            try
            {
                if (dataReader == null || dataReader.FieldCount == 0)
                    return default(TResult);

                if (!await dataReader.ReadAsync().ConfigureAwait(false))
                    return default(TResult);

                var values = new object[dataReader.FieldCount];
                dataReader.GetValues(values);
                var instance = SetMatchingValues(dataReader, values);
                while (await dataReader.NextResultAsync().ConfigureAwait(false))
                {
                }

                return instance;
            }
            finally
            {
                using (dataReader)
                {
                }
            }
        }
    }
}