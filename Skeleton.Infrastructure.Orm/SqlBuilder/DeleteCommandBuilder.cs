﻿using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core;

namespace Skeleton.Infrastructure.Orm.SqlBuilder
{
    internal sealed class DeleteCommandBuilder<TEntity> :
            SqlBuilderBase<TEntity>
        where TEntity : class, IEntity<TEntity>, new()
    {
        private CommandContext _context = new CommandContext();

        internal DeleteCommandBuilder(IMetadataProvider metadataProvider, TEntity entity)
            : base(metadataProvider)
        {
            Build(entity);
        }

        internal override string SqlQuery => SqlQueryTemplate
            .FormatWith(
                TableName,
                WhereCondition);

        protected internal override string SqlQueryTemplate => "DELETE FROM {0} {1}";

        private string WhereCondition => SqlFormatter.Conditions(_context.Conditions);

        protected internal override ContextBase ContextBase => _context;

        private void Build(TEntity entity)
        {
            QueryByPrimaryKey(e => e.Id.Equals(entity.Id));
        }

        internal override void OnNextQuery()
        {
            _context = new CommandContext();
        }
    }
}