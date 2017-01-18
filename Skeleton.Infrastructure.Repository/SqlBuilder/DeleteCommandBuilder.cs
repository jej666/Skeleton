using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    public sealed class DeleteCommandBuilder<TEntity> :
            SqlBuilderBase<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        private CommandContext _context = new CommandContext();

        public DeleteCommandBuilder(IMetadataProvider metadataProvider, TEntity entity)
            : base(metadataProvider)
        {
            Build(entity);
        }

        public override string SqlQuery => SqlQueryTemplate
            .FormatWith(
                TableName,
                WhereCondition);

        protected override string SqlQueryTemplate => "DELETE FROM {0} {1}";

        private string WhereCondition => SqlFormatter.Conditions(_context.Conditions);

        protected override ContextBase ContextBase => _context;

        private void Build(TEntity entity)
        {
            QueryByPrimaryKey(e => e.Id.Equals(entity.Id));
        }

        public override void OnNextQuery()
        {
            _context = new CommandContext();
        }
    }
}