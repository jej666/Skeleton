using System;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal static class SqlTemplate
    {
        private static string ScopeIdentity
        {
            get { return "SELECT scope_identity() AS [SCOPE_IDENTITY]"; }
        }

        internal static string ColumnValue(string tableName, string columnName, string value)
        {
            return "[{0}].[{1}] = {2} ".FormatWith(tableName, columnName, value);
        }

        internal static string Delete(string tableName, string conditions)
        {
            return "DELETE FROM {0} {1}".FormatWith(tableName, conditions);
        }

        internal static string Field(string tableName, string fieldName)
        {
            return "[{0}].[{1}]".FormatWith(tableName, fieldName);
        }

        internal static string Insert(string tableName, string columns, string values)
        {
            return "INSERT INTO {0} ({1}) VALUES ({2}); {3}".FormatWith(
                tableName, columns, values, ScopeIdentity);
        }

        internal static string PagedQuery(string selection, string source, string conditions, string order, int pageSize,
            int pageNumber)
        {
            return "SELECT {0} FROM {1} {2} {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY".FormatWith(
                selection, source, conditions, order, pageSize * (pageNumber - 1), pageSize);
        }

        internal static string Parameter(string parameterId)
        {
            return "@" + parameterId;
        }

        internal static string Query(string selection, string source, string conditions, string order, string grouping,
            string having)
        {
            return "SELECT {0} FROM {1} {2} {3} {4} {5}".FormatWith(
                selection, source, conditions, order, grouping, having);
        }

        internal static string Table(string tableName)
        {
            return "[{0}]".FormatWith(tableName);
        }

        internal static string Update(string tableName, string values, string conditions)
        {
            return "UPDATE {0} SET {1} {2}".FormatWith(
                tableName, values, conditions);
        }
    }
}