using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class QueryContext
    {
        private const string ParameterPrefix = "P";
        private readonly string _primaryTableName;
        private readonly string _primaryKeyName;
        private int _paramIndex;
        private string _top;

        private readonly LazyLoading<List<string>> _columns;
        private readonly LazyLoading<List<string>> _conditions;
        private readonly LazyLoading<List<string>> _groupBy;
        private readonly LazyLoading<List<string>> _having;
        private readonly LazyLoading<List<string>> _joins;
        private readonly LazyLoading<List<string>> _select;
        private readonly LazyLoading<List<string>> _orderBy;
        private readonly LazyLoading<List<string>> _tableNames;
        private readonly LazyLoading<List<string>> _updateValues;
        private readonly LazyLoading<List<string>> _insertValues;
        private readonly LazyLoading<IDictionary<string, object>> _parameters;

        internal QueryContext(string tableName, string primaryKeyName)
        {
            _primaryTableName = tableName;
            _primaryKeyName = primaryKeyName;

            _columns = new LazyLoading<List<string>>(() => new List<string>());
            _conditions = new LazyLoading<List<string>>(() => new List<string>());
            _groupBy = new LazyLoading<List<string>>(() => new List<string>());
            _having = new LazyLoading<List<string>>(() => new List<string>());
            _joins = new LazyLoading<List<string>>(() => new List<string>());
            _select = new LazyLoading<List<string>>(() => new List<string>());
            _orderBy = new LazyLoading<List<string>>(() => new List<string>());
            _tableNames = new LazyLoading<List<string>>(() => new List<string>());
            _insertValues = new LazyLoading<List<string>>(() => new List<string>());
            _updateValues = new LazyLoading<List<string>>(() => new List<string>());
            _parameters = new LazyLoading<IDictionary<string, object>>(() => new ExpandoObject());

            _tableNames.Value.Add(tableName);
        }

        internal IDictionary<string, object> Parameters
        {
            get { return _parameters.Value; }
        }

        internal string Top
        {
            get { return _top; }
        }

        internal IEnumerable<string> Columns
        {
            get { return _columns.Value; }
        }

        internal IEnumerable<string> Conditions
        {
            get { return _conditions.Value; }
        }

        internal IEnumerable<string> Grouping
        {
            get { return _groupBy.Value; }
        }

        internal IEnumerable<string> Having
        {
            get { return _having.Value; }
        }

        internal IEnumerable<string> InsertValues
        {
            get { return _insertValues.Value; }
        }

        internal string PrimaryTableName
        {
            get { return _primaryTableName; }
        }

        internal string PrimaryKeyName
        {
            get { return _primaryKeyName; }
        }

        internal IEnumerable<string> Source
        {
            get { return _joins.Value; }
        }

        internal IEnumerable<string> Selection
        {
            get { return _select.Value; }
        }

        internal IEnumerable<string> Order
        {
            get { return _orderBy.Value; }
        }

        internal IEnumerable<string> UpdateColumnValues
        {
            get { return _updateValues.Value; }
        }

        internal void AddTableName(string tableName)
        {
            _tableNames.Value.Add(tableName);
        }

        internal void AddJoin(string joinExpression)
        {
            _joins.Value.Add(joinExpression);
        }

        internal void AddCondition(string condition)
        {
            _conditions.Value.Add(condition);
        }

        internal void AddSelect(string select)
        {
            _select.Value.Add(select);
        }

        internal void AddInsert(string columnName, string formattedValue)
        {
            _columns.Value.Add(columnName);
            _insertValues.Value.Add(formattedValue);
        }

        internal void AddUpdate(string columnValue)
        {
            _updateValues.Value.Add(columnValue);
        }

        internal void And()
        {
            if (_conditions.Value.Count > 0)
                _conditions.Value.Add(" AND ");
        }

        internal void BeginExpression()
        {
            _conditions.Value.Add("(");
        }

        internal void EndExpression()
        {
            _conditions.Value.Add(")");
        }

        internal void AddGroupBy(string tableName, string fieldName)
        {
            _groupBy.Value.Add(SqlFormatter.Field(tableName, fieldName));
        }

        internal string NextParamId()
        {
            ++_paramIndex;

            return ParameterPrefix +
                _paramIndex.ToString(CultureInfo.InvariantCulture);
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

        internal void AddOrderBy(string tableName, string fieldName)
        {
            var order = SqlFormatter.Field(tableName, fieldName);

            _orderBy.Value.Add(order);
        }

        internal void AddOrderByDescending(string tableName, string fieldName)
        {
            var order = SqlFormatter.Field(tableName, fieldName);
            order += " DESC";

            _orderBy.Value.Add(order);
        }

        internal void AddSelectCount()
        {
            _select.Value.Add("Count(*)");
        }

        internal void AddSelectTop(int take)
        {
            _top = "Top({0})".FormatWith(take);
        }

        internal void AddParameter(string key, object value)
        {
            if (!Parameters.ContainsKey(key))
                Parameters.Add(key, value);
        }

        private class LazyLoading<T>
        {
            private Func<T> _initializer;
            private T _value;

            internal LazyLoading(Func<T> initializer)
            {
                initializer.ThrowIfNull(() => initializer);

                _initializer = initializer;
            }

            internal bool HasValue
            {
                get { return _initializer == null; }
            }

            internal T Value
            {
                get
                {
                    if (_initializer == null)
                        return _value;

                    _value = _initializer();
                    _initializer = null;

                    return _value;
                }
            }
        }
    }
}
