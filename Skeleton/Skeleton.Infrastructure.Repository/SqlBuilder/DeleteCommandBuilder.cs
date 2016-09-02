using System;
using Skeleton.Core;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class DeleteCommandBuilder<TEntity, TIdentity> :
            SqlBuilderBase<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private CommandContext _context = new CommandContext();

        internal DeleteCommandBuilder(IMetadataProvider metadataProvider, TEntity entity)
            : base(metadataProvider)
        {
            Build(entity);
        }

        internal override string SqlQuery
        {
            get
            {
                return SqlQueryTemplate
                    .FormatWith(
                        TableName,
                        WhereCondition);
            }
        }

        protected internal override string SqlQueryTemplate
        {
            get { return "DELETE FROM {0} {1}"; }
        }

        private string WhereCondition
        {
            get { return SqlFormatter.Conditions(_context.Conditions); }
        }

        protected internal override ContextBase ContextBase
        {
            get { return _context; }
        }

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