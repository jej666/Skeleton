using System;
using System.Data;

namespace Skeleton.Tests.Infrastructure
{
    internal static class SqlDataReaderHelper
    {
        internal static string GetNullableString(this IDataReader reader, int index)
        {
            object tmp = reader.GetValue(index);
            if (tmp != DBNull.Value)
            {
                return (string)tmp;
            }
            return null;
        }

        internal static Nullable<T> GetNullableValue<T>(this IDataReader reader, int index) where T : struct
        {
            object tmp = reader.GetValue(index);
            if (tmp != DBNull.Value)
            {
                return (T)tmp;
            }
            return null;
        }
    }
}