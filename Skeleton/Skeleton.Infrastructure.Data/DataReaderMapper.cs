namespace Skeleton.Infrastructure.Data
{
    using Common.Reflection;
    using System.Collections.Generic;
    using System.Data;

    internal sealed class DataReaderMapper<TResult> :
        DataReaderMapperBase<TResult>
        where TResult : class
    {
        internal DataReaderMapper(ITypeAccessorCache accessorCache)
            : base(accessorCache)
        { }

        internal IEnumerable<TResult> MapQuery(IDataReader dataReader)
        {
            try
            {
                var list = new List<TResult>();

                if (dataReader == null || dataReader.FieldCount == 0)
                    return list;

                while (dataReader.Read())
                {
                    var values = new object[dataReader.FieldCount];
                    dataReader.GetValues(values);

                    var instance = SetMatchingValues(dataReader, values);

                    list.Add(instance);
                }
                while (dataReader.NextResult()) { }

                return list;
            }
            finally
            {
                using (dataReader) { };
            }
        }

        internal TResult MapSingle(IDataReader dataReader)
        {
            try
            {
                if (dataReader == null || dataReader.FieldCount == 0)
                    return default(TResult);

                if (dataReader.Read())
                {
                    var values = new object[dataReader.FieldCount];
                    dataReader.GetValues(values);
                    var instance = SetMatchingValues(dataReader, values);
                    while (dataReader.NextResult()) { }

                    return instance;
                }

                return default(TResult);
            }
            finally
            {
                using (dataReader) { };
            }
        }
    }
}