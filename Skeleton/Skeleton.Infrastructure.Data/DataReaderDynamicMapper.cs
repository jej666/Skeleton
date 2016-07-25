using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace Skeleton.Infrastructure.Data
{
    internal static class DataReaderDynamicMapper
    {
        internal static IEnumerable<dynamic> Map(this IDataReader dataReader)
        {
            try
            {
                var list = new List<dynamic>();

                if (dataReader == null || dataReader.FieldCount == 0)
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

        private static dynamic SetValues(this IDataReader reader)
        {
            var values = new object[reader.FieldCount];
            var fieldCount = reader.GetValues(values);
            var expando = new ExpandoObject() as IDictionary<string, object>;

            for (var index = 0; index < fieldCount; ++index)
            {
                if (values[index] == null || values[index] is DBNull)
                    continue;

                expando.Add(reader.GetName(index), values[index].TrimIfNeeded());
            }

            return expando;
        }
    }
}