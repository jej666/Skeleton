//https://github.com/base33/lambda-sql-builder

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Repository.SqlBuilder.ExpressionTree;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class QueryBuilder
    {
        private const string PARAMETER_PREFIX = "P";
        private readonly LazyRef<List<string>> columns = new LazyRef<List<string>>(() => new List<string>());
        private readonly LazyRef<List<string>> conditions = new LazyRef<List<string>>(() => new List<string>());
        private readonly LazyRef<List<string>> groupingList = new LazyRef<List<string>>(() => new List<string>());
        private readonly LazyRef<List<string>> havingConditions = new LazyRef<List<string>>(() => new List<string>());
        private readonly LazyRef<List<string>> insertValues = new LazyRef<List<string>>(() => new List<string>());
        private readonly LazyRef<List<string>> joinExpressions = new LazyRef<List<string>>(() => new List<string>());
        private readonly LazyRef<List<string>> selectionList = new LazyRef<List<string>>(() => new List<string>());
        private readonly LazyRef<List<string>> sortList = new LazyRef<List<string>>(() => new List<string>());
        private readonly LazyRef<List<string>> tableNames = new LazyRef<List<string>>(() => new List<string>());
        private readonly LazyRef<List<string>> updateColumnValues = new LazyRef<List<string>>(() => new List<string>());

        private readonly Dictionary<ExpressionType, string> _operationDictionary =
            new Dictionary<ExpressionType, string>
            {
                {ExpressionType.Equal, "="},
                {ExpressionType.NotEqual, "!="},
                {ExpressionType.GreaterThan, ">"},
                {ExpressionType.LessThan, "<"},
                {ExpressionType.GreaterThanOrEqual, ">="},
                {ExpressionType.LessThanOrEqual, "<="}
            };

        private int _paramIndex;
        private readonly string _primaryTableName;
        private string _top;

        internal QueryBuilder(string tableName)
        {
            _primaryTableName = tableName;
            _tableNames.Add(tableName);

            Parameters = new ExpandoObject();
        }

        internal string DeleteQuery
        {
            get { return SqlTemplate.Delete(_primaryTableName, Conditions); }
        }

        internal string InsertQuery
        {
            get { return SqlTemplate.Insert(_primaryTableName, Columns, InsertValues); }
        }

        internal IDictionary<string, object> Parameters { get; private set; }

        internal string Query
        {
            get { return SqlTemplate.Query(Selection, Source, Conditions, Grouping, Having, Order); }
        }

        internal string UpdateQuery
        {
            get { return SqlTemplate.Update(_primaryTableName, UpdateColumnValues, Conditions); }
        }

        private List<string> _columns
        {
            get { return columns.Value; }
        }

        private List<string> _updateColumnValues
        {
            get { return updateColumnValues.Value; }
        }

        private List<string> _conditions
        {
            get { return conditions.Value; }
        }

        private List<string> _groupingList
        {
            get { return groupingList.Value; }
        }

        private List<string> _havingConditions
        {
            get { return havingConditions.Value; }
        }

        private List<string> _joinExpressions
        {
            get { return joinExpressions.Value; }
        }

        private List<string> _selectionList
        {
            get { return selectionList.Value; }
        }

        private List<string> _sortList
        {
            get { return sortList.Value; }
        }

        private List<string> _tableNames
        {
            get { return tableNames.Value; }
        }

        private List<string> _insertValues
        {
            get { return insertValues.Value; }
        }

        private string Columns
        {
            get { return string.Join(", ", _columns); }
        }

        private string UpdateColumnValues
        {
            get { return string.Join(", ", _updateColumnValues); }
        }

        private string Conditions
        {
            get
            {
                if (_conditions.Count == 0)
                    return "";
                return "WHERE " + string.Join("", _conditions);
            }
        }

        private string Grouping
        {
            get
            {
                if (_groupingList.Count == 0)
                    return "";
                return "GROUP BY " + string.Join(", ", _groupingList);
            }
        }

        private string Having
        {
            get
            {
                if (_havingConditions.Count == 0)
                    return "";
                return "HAVING " + string.Join(" ", _havingConditions);
            }
        }

        private string Order
        {
            get
            {
                if (_sortList.Count == 0)
                    return "";
                return "ORDER BY " + string.Join(", ", _sortList);
            }
        }

        private string Selection
        {
            get
            {
                var result = string.Empty;
                if (!string.IsNullOrEmpty(_top))
                    result = _top;
                if (_selectionList.Count == 0)
                    return result += "{0}.*".FormatWith(_primaryTableName);
                return result += string.Join(", ", _selectionList);
            }
        }

        private string Source
        {
            get
            {
                var joinExpression = string.Join(" ", _joinExpressions);
                return "{0} {1}".FormatWith(_primaryTableName, joinExpression);
            }
        }

        private string InsertValues
        {
            get { return string.Join(", ", _insertValues); }
        }

        internal void Insert(string columnName, object value)
        {
            _columns.Add(columnName);
            _insertValues.Add(GetFormattedValue(value));
        }

        internal void Update(string columnName, object value)
        {
            _updateColumnValues.Add(
                SqlTemplate.ColumnValue(
                    _primaryTableName,
                    columnName,
                    GetFormattedValue(value)));
        }

        internal void And()
        {
            if (_conditions.Count > 0)
                _conditions.Add(" AND ");
        }

        internal void BeginExpression()
        {
            _conditions.Add("(");
        }

        internal void BuildSql(Node node)
        {
            BuildSql((dynamic) node);
        }

        internal void BuildSql(LikeNode node)
        {
            if (node.Method == LikeMethod.Equals)
            {
                QueryByField(node.MemberNode.TableName, node.MemberNode.FieldName,
                    _operationDictionary[ExpressionType.Equal], node.Value);
            }
            else
            {
                var value = node.Value;
                switch (node.Method)
                {
                    case LikeMethod.StartsWith:
                        value = node.Value + "%";
                        break;

                    case LikeMethod.EndsWith:
                        value = "%" + node.Value;
                        break;

                    case LikeMethod.Contains:
                        value = "%" + node.Value + "%";
                        break;
                }
                QueryByFieldLike(
                    node.MemberNode.TableName,
                    node.MemberNode.FieldName, value);
            }
        }

        internal void BuildSql(OperationNode node)
        {
            BuildSql((dynamic) node.Left, (dynamic) node.Right, node.Operator);
        }

        internal void BuildSql(MemberNode memberNode)
        {
            QueryByField(
                memberNode.TableName,
                memberNode.FieldName,
                _operationDictionary[ExpressionType.Equal], true);
        }

        internal void BuildSql(SingleOperationNode node)
        {
            if (node.Operator == ExpressionType.Not)
                Not();

            BuildSql(node.Child);
        }

        internal void BuildSql(MemberNode memberNode, ValueNode valueNode, ExpressionType op)
        {
            if (valueNode.Value == null)
            {
                ResolveNullValue(memberNode, op);
            }
            else
            {
                QueryByField(
                    memberNode.TableName,
                    memberNode.FieldName,
                    _operationDictionary[op],
                    valueNode.Value);
            }
        }

        internal void BuildSql(ValueNode valueNode, MemberNode memberNode, ExpressionType op)
        {
            BuildSql(memberNode, valueNode, op);
        }

        internal void BuildSql(MemberNode leftMember, MemberNode rightMember, ExpressionType op)
        {
            QueryByFieldComparison(
                leftMember.TableName,
                leftMember.FieldName,
                _operationDictionary[op],
                rightMember.TableName,
                rightMember.FieldName);
        }

        internal void BuildSql(SingleOperationNode leftMember, Node rightMember, ExpressionType op)
        {
            if (leftMember.Operator == ExpressionType.Not)
                BuildSql(leftMember as Node, rightMember, op);
            else
                BuildSql((dynamic) leftMember.Child, (dynamic) rightMember, op);
        }

        internal void BuildSql(Node leftMember, SingleOperationNode rightMember, ExpressionType op)
        {
            BuildSql(rightMember, leftMember, op);
        }

        internal void BuildSql(Node leftNode, Node rightNode, ExpressionType op)
        {
            BeginExpression();
            BuildSql((dynamic) leftNode);
            ResolveOperation(op);
            BuildSql((dynamic) rightNode);
            EndExpression();
        }

        internal void EndExpression()
        {
            _conditions.Add(")");
        }

        internal void GroupBy(string tableName, string fieldName)
        {
            _groupingList.Add(SqlTemplate.Field(tableName, fieldName));
        }

        internal void Join(
            string originalTableName,
            string joinTableName,
            string leftField,
            string rightField)
        {
            var joinString = "JOIN {0} ON {1} = {2}".FormatWith(
                SqlTemplate.Table(joinTableName),
                SqlTemplate.Field(originalTableName, leftField),
                SqlTemplate.Field(joinTableName, rightField));
            _tableNames.Add(joinTableName);
            _joinExpressions.Add(joinString);
        }

        internal void Not()
        {
            _conditions.Add(" NOT ");
        }

        internal void Or()
        {
            if (_conditions.Count > 0)
                _conditions.Add(" OR ");
        }

        internal void OrderBy(string tableName, string fieldName)
        {
            var order = SqlTemplate.Field(tableName, fieldName);

            _sortList.Add(order);
        }

        internal void OrderByDescending(string tableName, string fieldName)
        {
            var order = SqlTemplate.Field(tableName, fieldName);
            order += " DESC";

            _sortList.Add(order);
        }

        internal string PagedQuery(int pageSize, int pageNumber)
        {
            if (_sortList.Count == 0)
                throw new ArgumentException("Pagination requires the ORDER BY statement to be specified");

            return SqlTemplate.PagedQuery(Selection, Source, Conditions, Order, pageSize, pageNumber);
        }

        internal void QueryByField(string tableName, string fieldName, string op, object fieldValue)
        {
            var paramId = NextParamId();
            var newCondition = "{0} {1} {2}".FormatWith(
                SqlTemplate.Field(tableName, fieldName),
                op,
                SqlTemplate.Parameter(paramId));

            _conditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        internal void QueryByFieldComparison(string leftTableName, string leftFieldName, string op,
            string rightTableName, string rightFieldName)
        {
            var newCondition = "{0} {1} {2}".FormatWith(
                SqlTemplate.Field(leftTableName, leftFieldName),
                op,
                SqlTemplate.Field(rightTableName, rightFieldName));

            _conditions.Add(newCondition);
        }

        internal void QueryByFieldLike(string tableName, string fieldName, string fieldValue)
        {
            var paramId = NextParamId();
            var newCondition = "{0} LIKE {1}".FormatWith(
                SqlTemplate.Field(tableName, fieldName),
                SqlTemplate.Parameter(paramId));

            _conditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        internal void QueryByFieldNotNull(string tableName, string fieldName)
        {
            _conditions.Add("{0} IS NOT NULL"
                .FormatWith(
                    SqlTemplate.Field(tableName, fieldName)));
        }

        internal void QueryByFieldNull(string tableName, string fieldName)
        {
            _conditions.Add("{0} IS NULL"
                .FormatWith(
                    SqlTemplate.Field(tableName, fieldName)));
        }

        internal void QueryByIsIn(string tableName, string fieldName, ISqlQuery sqlQuery)
        {
            var innerQuery = sqlQuery.Query;
            foreach (var param in sqlQuery.Parameters)
            {
                var innerParamKey = "Inner" + param.Key;
                innerQuery = Regex.Replace(innerQuery, param.Key, innerParamKey);
                AddParameter(innerParamKey, param.Value);
            }

            var newCondition = "{0} IN ({1})".FormatWith(
                SqlTemplate.Field(tableName, fieldName),
                innerQuery);

            _conditions.Add(newCondition);
        }

        internal void QueryByIsIn(string tableName, string fieldName, IEnumerable<object> values)
        {
            var paramIds = values.Select(x =>
            {
                var paramId = NextParamId();
                AddParameter(paramId, x);
                return SqlTemplate.Parameter(paramId);
            });

            var newCondition = "{0} IN ({1})".FormatWith(
                SqlTemplate.Field(tableName, fieldName),
                string.Join(",", paramIds));

            _conditions.Add(newCondition);
        }

        internal void Select(string tableName)
        {
            var selectionString = "{0}.*".FormatWith(SqlTemplate.Table(tableName));
            _selectionList.Add(selectionString);
        }

        internal void Select(string tableName, string fieldName)
        {
            _selectionList.Add(SqlTemplate.Field(tableName, fieldName));
        }

        internal void Select(string tableName, string fieldName, SelectFunction selectFunction)
        {
            var selectionString = "{0}({1})".FormatWith(
                selectFunction.ToString(),
                SqlTemplate.Field(tableName, fieldName));

            _selectionList.Add(selectionString);
        }

        internal void SelectTop(int take)
        {
            _top = "Top({0})".FormatWith(take);
        }

        private static string GetFormattedValue(object value)
        {
            var formattedValue = string.Empty;

            if (value is string)
            {
                formattedValue = "'{0}'".FormatWith(value.ToString().Replace("'", "''"));
            }
            else if (value != null && value.ToString().Contains(","))
            {
                formattedValue = value.ToString().Replace(",", ".");
            }
            else
            {
                formattedValue = value != null ? value.ToString() : "null";
            }

            return formattedValue;
        }

        private void AddParameter(string key, object value)
        {
            if (!Parameters.ContainsKey(key))
                Parameters.Add(key, value);
        }

        private string NextParamId()
        {
            ++_paramIndex;
            return PARAMETER_PREFIX + _paramIndex.ToString(CultureInfo.InvariantCulture);
        }

        private void ResolveNullValue(MemberNode memberNode, ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.Equal:
                    QueryByFieldNull(memberNode.TableName, memberNode.FieldName);
                    break;

                case ExpressionType.NotEqual:
                    QueryByFieldNotNull(memberNode.TableName, memberNode.FieldName);
                    break;
            }
        }

        private void ResolveOperation(ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    And();
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    Or();
                    break;

                default:
                    throw new ArgumentException(
                        "Unrecognized binary expression operation '{0}'"
                            .FormatWith(op.ToString()));
            }
        }
    }
}