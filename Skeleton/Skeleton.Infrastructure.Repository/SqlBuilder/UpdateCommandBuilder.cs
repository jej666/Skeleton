using System;
using Skeleton.Abstraction;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class UpdateCommandBuilder<TEntity, TIdentity> :
            SqlBuilderBase<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private CommandContext _context = new CommandContext();

        internal UpdateCommandBuilder(IMetadataProvider metadataProvider, TEntity entity)
            : base(metadataProvider)
        {
            Build(entity);
        }

        internal override string SqlQuery => SqlQueryTemplate
            .FormatWith(
                TableName,
                UpdateColumnValues,
                WhereCondition);

        private string UpdateColumnValues => SqlFormatter.Fields(_context.ColumnValues);

        private string WhereCondition => SqlFormatter.Conditions(_context.Conditions);

        protected internal override string SqlQueryTemplate => "UPDATE {0} SET {1} {2}";

        protected internal override ContextBase ContextBase => _context;

        private void Build(TEntity entity)
        {
            foreach (var column in GetTableColumns(entity))
            {
                if (entity.IdAccessor.Name.IsNullOrEmpty() ||
                    (entity.IdAccessor.Name == column.Name))
                    continue;

                var value = column.GetValue(entity);
                var columnValue = GetUpdateColumnValue(column, value);

                _context.ColumnValues.Add(columnValue);
            }

            QueryByPrimaryKey(e => e.Id.Equals(entity.Id));
        }

        private string GetUpdateColumnValue(IMemberAccessor column, object value)
        {
            return SqlFormatter.ColumnValue(TableName, column.Name, value);
        }

        internal override void OnNextQuery()
        {
            _context = new CommandContext();
        }
    }
}