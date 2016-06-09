using System;
using System.Data;

namespace Skeleton.Tests.Infrastructure
{
    public static class SqlDataReaderHelper
    {
        public static string GetNullableString(this IDataReader reader, int index)
        {
            var tmp = reader.GetValue(index);
            if (tmp != DBNull.Value)
            {
                return (string) tmp;
            }
            return null;
        }

        public static T? GetNullableValue<T>(this IDataReader reader, int index) where T : struct
        {
            var tmp = reader.GetValue(index);
            if (tmp != DBNull.Value)
            {
                return (T) tmp;
            }
            return null;
        }
    }
}