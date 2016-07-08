﻿//https://github.com/base33/lambda-sql-builder

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class SqlQueryBuilder
    {
        private const string ParameterPrefix = "P";
        private readonly LazyLoading<List<string>> _columns = new LazyLoading<List<string>>(() => new List<string>());
        private readonly LazyLoading<List<string>> _conditions = new LazyLoading<List<string>>(() => new List<string>());

        private readonly LazyLoading<List<string>> _groupingList =
            new LazyLoading<List<string>>(() => new List<string>());

        private readonly LazyLoading<List<string>> _havingConditions =
            new LazyLoading<List<string>>(() => new List<string>());

        private readonly LazyLoading<List<string>> _insertValues =
            new LazyLoading<List<string>>(() => new List<string>());

        private readonly LazyLoading<List<string>> _joinExpressions =
            new LazyLoading<List<string>>(() => new List<string>());

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

        private readonly string _primaryTableName;

        private readonly LazyLoading<List<string>> _selectionList =
            new LazyLoading<List<string>>(() => new List<string>());

        private readonly LazyLoading<List<string>> _sortList = new LazyLoading<List<string>>(() => new List<string>());
        private readonly LazyLoading<List<string>> _tableNames = new LazyLoading<List<string>>(() => new List<string>());

        private readonly LazyLoading<List<string>> _updateColumnValues =
            new LazyLoading<List<string>>(() => new List<string>());

        private int _paramIndex;
        private string _top;

        internal SqlQueryBuilder(string tableName)
        {
            _primaryTableName = tableName;
            _tableNames.Value.Add(tableName);

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

        private string Columns
        {
            get { return string.Join(", ", _columns.Value); }
        }

        private string UpdateColumnValues
        {
            get { return string.Join(", ", _updateColumnValues.Value); }
        }

        private string Conditions
        {
            get
            {
                if (_conditions.Value.Count == 0)
                    return "";
                return "WHERE " + string.Join("", _conditions.Value);
            }
        }

        private string Grouping
        {
            get
            {
                if (_groupingList.Value.Count == 0)
                    return "";
                return "GROUP BY " + string.Join(", ", _groupingList.Value);
            }
        }

        private string Having
        {
            get
            {
                if (_havingConditions.Value.Count == 0)
                    return "";
                return "HAVING " + string.Join(" ", _havingConditions.Value);
            }
        }

        private string Order
        {
            get
            {
                if (_sortList.Value.Count == 0)
                    return "";
                return "ORDER BY " + string.Join(", ", _sortList.Value);
            }
        }

        private string Selection
        {
            get
            {
                var result = string.Empty;
                if (!string.IsNullOrEmpty(_top))
                    result = _top;

                if (_selectionList.Value.Count == 0)
                    return result += "{0}.*".FormatWith(_primaryTableName);

                return result += string.Join(", ", _selectionList.Value);
            }
        }

        private string Source
        {
            get
            {
                var joinExpression = string.Join(" ", _joinExpressions.Value);
                return "{0} {1}".FormatWith(_primaryTableName, joinExpression);
            }
        }

        private string InsertValues
        {
            get { return string.Join(", ", _insertValues.Value); }
        }

        internal void Insert(string columnName, object value)
        {
            _columns.Value.Add(columnName);
            _insertValues.Value.Add(GetFormattedValue(value));
        }

        internal void Update(string columnName, object value)
        {
            _updateColumnValues.Value.Add(
                SqlTemplate.ColumnValue(
                    _primaryTableName,
                    columnName,
                    GetFormattedValue(value)));
        }

        internal void And()
        {
            if (_conditions.Value.Count > 0)
                _conditions.Value.Add(" AND ");
        }

        private void BeginExpression()
        {
            _conditions.Value.Add("(");
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

        private void BuildSql(MemberNode memberNode, ValueNode valueNode, ExpressionType op)
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

        private void BuildSql(ValueNode valueNode, MemberNode memberNode, ExpressionType op)
        {
            BuildSql(memberNode, valueNode, op);
        }

        private void BuildSql(MemberNode leftMember, MemberNode rightMember, ExpressionType op)
        {
            QueryByFieldComparison(
                leftMember.TableName,
                leftMember.FieldName,
                _operationDictionary[op],
                rightMember.TableName,
                rightMember.FieldName);
        }

        private void BuildSql(SingleOperationNode leftMember, Node rightMember, ExpressionType op)
        {
            if (leftMember.Operator == ExpressionType.Not)
                BuildSql(leftMember as Node, rightMember, op);
            else
                BuildSql((dynamic) leftMember.Child, (dynamic) rightMember, op);
        }

        private void BuildSql(Node leftMember, SingleOperationNode rightMember, ExpressionType op)
        {
            BuildSql(rightMember, leftMember, op);
        }

        private void BuildSql(Node leftNode, Node rightNode, ExpressionType op)
        {
            BeginExpression();
            BuildSql((dynamic) leftNode);
            ResolveOperation(op);
            BuildSql((dynamic) rightNode);
            EndExpression();
        }

        private void EndExpression()
        {
            _conditions.Value.Add(")");
        }

        internal void GroupBy(string tableName, string fieldName)
        {
            _groupingList.Value.Add(SqlTemplate.Field(tableName, fieldName));
        }

        internal void Join(
            string originalTableName,
            string joinTableName,
            string leftField,
            string rightField,
            JoinType joinType)
        {
            var joinString = " {0} JOIN {1} ON {2} = {3}".FormatWith(
                joinType.ToString(),
                SqlTemplate.Table(joinTableName),
                SqlTemplate.Field(originalTableName, leftField),
                SqlTemplate.Field(joinTableName, rightField));
            _tableNames.Value.Add(joinTableName);
            _joinExpressions.Value.Add(joinString);
        }

        internal void Not()
        {
            _conditions.Value.Add(" NOT ");
        }

        internal void Or()
        {
            if (_conditions.Value.Count > 0)
                _conditions.Value.Add(" OR ");
        }

        internal void OrderBy(string tableName, string fieldName)
        {
            var order = SqlTemplate.Field(tableName, fieldName);

            _sortList.Value.Add(order);
        }

        internal void OrderByDescending(string tableName, string fieldName)
        {
            var order = SqlTemplate.Field(tableName, fieldName);
            order += " DESC";

            _sortList.Value.Add(order);
        }

        internal string PagedQuery(int pageSize, int pageNumber)
        {
            if (_sortList.Value.Count == 0)
                throw new ArgumentException("Pagination requires the ORDER BY statement to be specified");

            return SqlTemplate.PagedQuery(Selection, Source, Conditions, Order, pageSize, pageNumber);
        }

        private void QueryByField(string tableName, string fieldName, string op, object fieldValue)
        {
            var paramId = NextParamId();
            var newCondition = "{0} {1} {2}".FormatWith(
                SqlTemplate.Field(tableName, fieldName),
                op,
                SqlTemplate.Parameter(paramId));

            _conditions.Value.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        private void QueryByFieldComparison(string leftTableName, string leftFieldName, string op,
            string rightTableName, string rightFieldName)
        {
            var newCondition = "{0} {1} {2}".FormatWith(
                SqlTemplate.Field(leftTableName, leftFieldName),
                op,
                SqlTemplate.Field(rightTableName, rightFieldName));

            _conditions.Value.Add(newCondition);
        }

        private void QueryByFieldLike(string tableName, string fieldName, string fieldValue)
        {
            var paramId = NextParamId();
            var newCondition = "{0} LIKE {1}".FormatWith(
                SqlTemplate.Field(tableName, fieldName),
                SqlTemplate.Parameter(paramId));

            _conditions.Value.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        private void QueryByFieldNotNull(string tableName, string fieldName)
        {
            _conditions.Value.Add("{0} IS NOT NULL"
                .FormatWith(
                    SqlTemplate.Field(tableName, fieldName)));
        }

        private void QueryByFieldNull(string tableName, string fieldName)
        {
            _conditions.Value.Add("{0} IS NULL"
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

            _conditions.Value.Add(newCondition);
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

            _conditions.Value.Add(newCondition);
        }

        internal void Select(string tableName)
        {
            var selectionString = "{0}.*".FormatWith(SqlTemplate.Table(tableName));
            _selectionList.Value.Add(selectionString);
        }

        internal void Select(string tableName, string fieldName)
        {
            _selectionList.Value.Add(SqlTemplate.Field(tableName, fieldName));
        }

        internal void Select(string tableName, string fieldName, SelectFunction selectFunction)
        {
            var selectionString = "{0}({1})".FormatWith(
                selectFunction.ToString(),
                SqlTemplate.Field(tableName, fieldName));

            _selectionList.Value.Add(selectionString);
        }

        internal void SelectCount()
        {
            _selectionList.Value.Add("Count(*)");
        }

        internal void SelectTop(int take)
        {
            _top = "Top({0})".FormatWith(take);
        }

        private static string GetFormattedValue(object value)
        {
            if (value is string)
                return "'{0}'".FormatWith(value.ToString().Replace("'", "''"));

            if (value != null && value.ToString().Contains(","))
                return value.ToString().Replace(",", ".");

            return value != null ? value.ToString() : "null";
        }

        private void AddParameter(string key, object value)
        {
            if (!Parameters.ContainsKey(key))
                Parameters.Add(key, value);
        }

        private string NextParamId()
        {
            ++_paramIndex;
            return ParameterPrefix + _paramIndex.ToString(CultureInfo.InvariantCulture);
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