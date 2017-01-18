using System.Collections.Generic;
using System.Data;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Data
{
    internal sealed class DataReaderMapper<TPoco> :
            DataReaderMapperBase<TPoco>
        where TPoco : class
    {
        internal DataReaderMapper(IMetadataProvider accessorCache)
            : base(accessorCache)
        {
        }

        internal IEnumerable<TPoco> MapQuery(IDataReader dataReader)
        {
            try
            {
                var list = new List<TPoco>();

                if ((dataReader == null) || (dataReader.FieldCount == 0))
                    return list;

                while (dataReader.Read())
                {
                    var values = new object[dataReader.FieldCount];
                    dataReader.GetValues(values);

                    var instance = SetMatchingValues(dataReader, values);

                    list.Add(instance);
                }
                while (dataReader.NextResult())
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

        internal TPoco MapSingle(IDataReader dataReader)
        {
            try
            {
                if ((dataReader == null) || (dataReader.FieldCount == 0))
                    return default(TPoco);

                if (!dataReader.Read())
                    return default(TPoco);

                var values = new object[dataReader.FieldCount];
                dataReader.GetValues(values);
                var instance = SetMatchingValues(dataReader, values);

                while (dataReader.NextResult())
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