using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class PagedSelectQueryBuilder<TEntity> :
        SelectQueryBuilder<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        private const string _sqlPagedQueryTemplate =
          "SELECT {0} FROM {1} {2} {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY";

        private readonly int _pageSize;
        private readonly int _pageNumber;

        internal PagedSelectQueryBuilder(IMetadataProvider metadataProvider, int pageSize, int pageNumber)
            : base(metadataProvider)
        {
            _pageSize = pageSize;
            _pageNumber = pageNumber;
        }

        internal override string SqlQuery
        {
            get
            {
                if (Context.OrderBy.IsNullOrEmpty())
                    OrderByPrimaryKey();

                return _sqlPagedQueryTemplate
                    .FormatWith(
                        SqlFormatter.SelectedColumns(Context.Selection, TableName),
                        SqlFormatter.Source(Context.Source, TableName),
                        SqlFormatter.Conditions(Context.Conditions),
                        SqlFormatter.OrderBy(Context.OrderBy),
                        _pageSize * (_pageNumber - 1),
                        _pageSize);
            }
        }
    }
}