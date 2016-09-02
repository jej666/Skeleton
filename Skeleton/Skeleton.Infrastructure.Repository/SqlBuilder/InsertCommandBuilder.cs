using System;
using Skeleton.Core;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class InsertCommandBuilder<TEntity, TIdentity> :
            SqlBuilderBase<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private const string ScopeIdentity = "SELECT scope_identity() AS[SCOPE_IDENTITY]";
        private CommandContext _context = new CommandContext();

        internal InsertCommandBuilder(IMetadataProvider metadataProvider, TEntity entity)
            : base(metadataProvider)
        {
            Build(entity);
        }

        internal override string SqlQuery => SqlQueryTemplate
            .FormatWith(
                TableName,
                InsertColumns,
                InsertValues,
                ScopeIdentity);

        private string InsertColumns => SqlFormatter.Fields(_context.Columns);

        private string InsertValues => SqlFormatter.Fields(_context.Values);

        protected internal override string SqlQueryTemplate => "INSERT INTO {0} ({1}) VALUES ({2}); {3};";

        protected internal override ContextBase ContextBase => _context;

        private void Build(TEntity entity)
        {
            foreach (var column in GetTableColumns(entity))
            {
                if (entity.IdAccessor.Name.IsNullOrEmpty() ||
                    (entity.IdAccessor.Name == column.Name))
                    continue;

                var value = column.GetValue(entity);
                var formattedValue = SqlFormatter.FormattedValue(value);

                _context.Columns.Add(column.Name);
                _context.Values.Add(formattedValue);
            }
        }

        internal override void OnNextQuery()
        {
            _context = new CommandContext();
        }
    }
}