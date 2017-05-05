using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using Skeleton.Common;
using System;
using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Orm.SqlBuilder
{
    internal class QueryEvaluator<TEntity> 
        where TEntity : class, IEntity<TEntity>
    {
        private readonly SelectQueryBuilder<TEntity> _builder;
        private readonly IQuery _query;

        internal QueryEvaluator(SelectQueryBuilder<TEntity> builder, IQuery query)
        {
            _builder = builder;
            _query = query;
        }

        internal bool Pagination => _query.PageSize.HasValue && _query.PageNumber.HasValue;
        internal bool OrderBy => _query.OrderBy != null && _query.OrderBy.IsNotNullOrEmpty();
        internal bool FieldsSelected => _query.Fields != null && _query.Fields.IsNotNullOrEmpty();

        internal void Evaluate()
        {
            if (Pagination)
            {
                _builder.Context.PageNumber = _query.PageNumber.Value;
                _builder.Context.PageSize = _query.PageSize.Value;

                if (!OrderBy)
                      _builder.OrderByPrimaryKey();
            }

            if (OrderBy)
                EvaluateOrderBy();

            if (FieldsSelected)
                EvaluateFields();
        }

        private void EvaluateFields()
        {
            var fields = _query.Fields.Split(',');
            foreach (var field in fields)
            {
                var evaluatedExpression = EvaluateProperty(field);
                _builder.Select(evaluatedExpression);
            }
        }

        private void EvaluateOrderBy()
        {
            var orderByFields = _query.OrderBy.Split(',');
            foreach (var field in orderByFields)
            {
                if (field.StartsWith("-", StringComparison.InvariantCultureIgnoreCase))
                {
                    var propertyName = field.Substring(1);
                    var evaluatedExpression = EvaluateProperty(propertyName);
                    _builder.OrderByDescending(evaluatedExpression);
                }
                else
                {
                    var evaluatedExpression = EvaluateProperty(field);
                    _builder.OrderBy(evaluatedExpression);
                }
            }
        }

        private LambdaExpression EvaluateProperty(string propertyName)
        {
            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, "p");
            var property = Expression.Property(parameter, propertyName);

            return Expression.Lambda(property, parameter);
        }
    }
}
