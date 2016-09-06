using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Common;
using Skeleton.Infrastructure.Repository.ExpressionTree;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal static class SqlFormatter
    {
        private const string FieldDelimiter = ", ";

        internal static Dictionary<ExpressionType, string> Operations => new Dictionary<ExpressionType, string>
        {
            {ExpressionType.Equal, "="},
            {ExpressionType.NotEqual, "!="},
            {ExpressionType.GreaterThan, ">"},
            {ExpressionType.LessThan, "<"},
            {ExpressionType.GreaterThanOrEqual, ">="},
            {ExpressionType.LessThanOrEqual, "<="}
        };
        
        internal static string CountAny => "Count(*)";

        internal static string BeginExpression => "(";

        internal static string EndExpression => ")";

        internal static string AndExpression => " AND ";

        internal static string NotExpression => " NOT ";

        internal static string OrExpression => " OR ";

        internal static string ColumnValue(string tableName, string columnName, object value)
        {
            return "[{0}].[{1}] = {2} ".FormatWith(tableName, columnName, FormattedValue(value));
        }

        internal static string Top(int take)
        {
            return "Top({0})".FormatWith(take);
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

        internal static string OrderByDescending(MemberNode node)
        {
            return "{0} DESC".FormatWith(Field(node));
        }

        internal static string Source(IEnumerable<string> source, string tableName)
        {
            return "{0} {1}".FormatWith(tableName, string.Join(" ", source));
        }

        internal static string Conditions(IEnumerable<string> where)
        {
            return where.IsNullOrEmpty() ? string.Empty : "WHERE {0}".FormatWith(string.Join("", where));
        }

        internal static string GroupBy(IEnumerable<string> grouping)
        {
            return grouping.IsNullOrEmpty() ? string.Empty : "GROUP BY {0}".FormatWith(Fields(grouping));
        }

        internal static string Having(IEnumerable<string> having)
        {
            return having.IsNullOrEmpty() ? string.Empty : "HAVING {0}".FormatWith(string.Join(" ", having));
        }

        internal static string OrderBy(IEnumerable<string> ordering)
        {
            return ordering.IsNullOrEmpty() ? string.Empty : "ORDER BY {0}".FormatWith(Fields(ordering));
        }

        internal static string SelectAll(string tableName)
        {
            return "{0}.*".FormatWith(Table(tableName));
        }

        internal static string SelectedColumns(IEnumerable<string> selection, string tableName)
        {
            return selection.IsNullOrEmpty() ? SelectAll(tableName) : Fields(selection);
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
            return "{0}({1}) AS {0}".FormatWith(selectFunction, Field(node));
        }

        internal static string WhereIsIn(MemberNode node, IEnumerable<string> parameterIds)
        {
            return "{0} IN ({1})".FormatWith(
                Field(node),
                Fields(parameterIds));
        }

        internal static string LikeCondition(string value, LikeMethod method)
        {
            switch (method)
            {
                case LikeMethod.StartsWith:
                    return value + "%";

                case LikeMethod.EndsWith:
                    return "%" + value;

                case LikeMethod.Contains:
                    return "%" + value + "%";

                default:
                    return string.Empty;
            }
        }

        internal static string FormattedValue(object value)
        {
            if (value is string)
                return "'{0}'".FormatWith(value.ToString().Replace("'", "''"));

            if ((value != null) && value.ToString().Contains(","))
                return value.ToString().Replace(",", ".");

            return value?.ToString() ?? "null";
        }
    }
}