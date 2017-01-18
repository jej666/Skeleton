using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Data
{
    internal sealed class DataReaderMapperAsync<TPoco> :
            DataReaderMapperBase<TPoco>
        where TPoco : class
    {
        internal DataReaderMapperAsync(IMetadataProvider accessorCache)
            : base(accessorCache)
        {
        }

        internal async Task<IEnumerable<TPoco>> MapQueryAsync(DbDataReader dataReader)
        {
            try
            {
                var list = new List<TPoco>();

                if ((dataReader == null) || (dataReader.FieldCount == 0))
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

        internal async Task<TPoco> MapSingleAsync(DbDataReader dataReader)
        {
            try
            {
                if ((dataReader == null) || (dataReader.FieldCount == 0))
                    return default(TPoco);

                if (!await dataReader.ReadAsync().ConfigureAwait(false))
                    return default(TPoco);

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