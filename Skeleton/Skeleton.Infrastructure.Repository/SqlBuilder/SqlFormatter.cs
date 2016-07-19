using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal static class SqlFormatter
    {
        private const string FieldDelimiter = ", ";

        internal static Dictionary<ExpressionType, string> Operations
        {
            get
            {
                return new Dictionary<ExpressionType, string>
                {
                    {ExpressionType.Equal, "="},
                    {ExpressionType.NotEqual, "!="},
                    {ExpressionType.GreaterThan, ">"},
                    {ExpressionType.LessThan, "<"},
                    {ExpressionType.GreaterThanOrEqual, ">="},
                    {ExpressionType.LessThanOrEqual, "<="}
                };
            }
        }

        internal static string ColumnValue(string tableName, string columnName, object value)
        {
            return "[{0}].[{1}] = {2} ".FormatWith(tableName, columnName, GetFormattedValue(value));
        }

        internal static string DeleteQuery
        {
            get { return "DELETE FROM {0} {1}"; }
        }

        internal static string InsertQuery
        {
            get { return "INSERT INTO {0} ({1}) VALUES ({2}); SELECT scope_identity() AS [SCOPE_IDENTITY]"; }
        }

        internal static string SelectQuery
        {
            get { return "SELECT {0} {1} FROM {2} {3} {4} {5} {6}"; }
        }

        internal static string UpdateQuery
        {
            get { return "UPDATE {0} SET {1} {2}"; }
        }

        internal static string PagedQuery
        {
            get { return "SELECT {0} FROM {1} {2} {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY"; }
        }

        internal static string Field(string tableName, string fieldName)
        {
            return Field(new MemberNode
            {
                TableName = tableName,
                FieldName = fieldName
            });
        }

        internal static string Field(MemberNode node)
        {
            return "[{0}].[{1}]".FormatWith(node.TableName, node.FieldName);
        }

        internal static string Parameter(string parameterId)
        {
            return "@" + parameterId;
        }

        internal static string Table(string tableName)
        {
            return "[{0}]".FormatWith(tableName);
        }

        internal static string Fields(IEnumerable<string> fields)
        {
            return string.Join(FieldDelimiter, fields);
        }

        internal static string Source(IEnumerable<string> source, string tableName)
        {
            var joinExpression = string.Join(" ", source);
            return "{0} {1}".FormatWith(tableName, joinExpression);
        }

        internal static string Conditions(IEnumerable<string> where)
        {
            if (where.IsNullOrEmpty())
                return string.Empty;

            return "WHERE {0}".FormatWith(string.Join("", where));
        }

        internal static string Grouping(IEnumerable<string> grouping)
        {
            if (grouping.IsNullOrEmpty())
                return string.Empty;

            return "GROUP BY {0}".FormatWith(Fields(grouping));
        }

        internal static string Having(IEnumerable<string> having)
        {
            if (having.IsNullOrEmpty())
                return string.Empty;

            return "HAVING {0}".FormatWith(string.Join(" ", having));
        }

        internal static string Ordering(IEnumerable<string> ordering)
        {
            if (ordering.IsNullOrEmpty())
                return string.Empty;

            return "ORDER BY {0}".FormatWith(Fields(ordering));
        }

        internal static string Selection(IEnumerable<string> selection,  string tableName)
        {
            if (selection.IsNullOrEmpty())
                return "{0}.*".FormatWith(tableName);

            return Fields(selection);
        }

        internal static string Join(
            string originalTableName,
            string joinTableName,
            string leftField,
            string rightField,
            JoinType joinType)
        {
            return " {0} JOIN {1} ON {2} = {3}".FormatWith(
                    joinType.ToString(),
                    Table(joinTableName),
                    Field(originalTableName, leftField),
                    Field(joinTableName, rightField));
        }

        internal static string FieldCondition(MemberNode node, string op, string parameterId)
        {
            return "{0} {1} {2}".FormatWith(
                 Field(node),
                 op,
                 Parameter(parameterId));
        }

        internal static string FieldComparison(MemberNode leftNode, string op, MemberNode rightNode)
        {
            return "{0} {1} {2}".FormatWith(Field(leftNode), op, Field(rightNode));
        }

        internal static string FieldLike(MemberNode node, string parameterId)
        {
            return "{0} LIKE {1}".FormatWith(
                Field(node),
                Parameter(parameterId));
        }

        internal static string FieldIsNotNull(MemberNode node)
        {
            return "{0} IS NOT NULL".FormatWith(Field(node));
        }

        internal static string FieldIsNull(MemberNode node)
        {
            return "{0} IS NULL".FormatWith(Field(node));
        }

        internal static string SelectAggregate(MemberNode node, string selectFunction)
        {
            return "{0}({1})".FormatWith(selectFunction, Field(node));
        }

        internal static string QueryIsIn(MemberNode node, IEnumerable<string> parameterIds)
        {
            return "{0} IN ({1})".FormatWith(
                 Field(node),
                 Fields(parameterIds));
        }

        internal static string GetFormattedValue(object value)
        {
            if (value is string)
                return "'{0}'".FormatWith(value.ToString().Replace("'", "''"));

            if (value != null && value.ToString().Contains(","))
                return value.ToString().Replace(",", ".");

            return value != null ? value.ToString() : "null";
        }
    }
}