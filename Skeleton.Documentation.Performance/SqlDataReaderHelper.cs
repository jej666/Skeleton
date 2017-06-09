using Skeleton.Common;
using System;
using System.Data;

namespace Skeleton.Documentation.Performance
{
    public static class SqlDataReaderHelper
    {
        public static string GetNullableString(this IDataReader reader, int index)
        {
            reader.ThrowIfNull(nameof(reader));

            var tmp = reader.GetValue(index);
            if (tmp != DBNull.Value)
                return (string)tmp;
            return null;
        }

        public static T? GetNullableValue<T>(this IDataReader reader, int index) where T : struct
        {
            reader.ThrowIfNull(nameof(reader));

            var tmp = reader.GetValue(index);
            if (tmp != DBNull.Value)
                return (T)tmp;
            return null;
        }
    }
}