using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Data
{
    internal static class DataReaderDynamicMapper
    {
        internal static IEnumerable<dynamic> Map(this IDataReader dataReader)
        {
            try
            {
                var list = new List<dynamic>();

                if ((dataReader == null) || (dataReader.FieldCount == 0))
                    return list;

                while (dataReader.Read())
                {
                    var row = dataReader.SetValues();
                    list.Add(row);
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

        internal static async Task<IEnumerable<dynamic>> Map(this DbDataReader dataReader)
        {
            try
            {
                var list = new List<dynamic>();

                if ((dataReader == null) || (dataReader.FieldCount == 0))
                    return list;

                while (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    var instance = SetValues(dataReader);

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

        private static dynamic SetValues(this IDataReader reader)
        {
            var values = new object[reader.FieldCount];
            var fieldCount = reader.GetValues(values);
            var dynamicDictionary = new ExpandoObject() as IDictionary<string, object>;

            for (var index = 0; index < fieldCount; ++index)
            {
                if ((values[index] == null) || values[index] is DBNull)
                    continue;

                dynamicDictionary.Add(reader.GetName(index), values[index].TrimIfNeeded());
            }

            return dynamicDictionary;
        }
    }
}