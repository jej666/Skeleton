using System.Collections.Generic;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class QueryContext : ContextBase
    {
        private readonly LazyLoading<List<string>> _groupBy;
        private readonly LazyLoading<List<string>> _having;
        private readonly LazyLoading<List<string>> _joins;
        private readonly LazyLoading<List<string>> _select;
        private readonly LazyLoading<List<string>> _orderBy;
        private readonly LazyLoading<List<string>> _tableNames;

        internal QueryContext()
        {
            _groupBy = new LazyLoading<List<string>>(() => new List<string>());
            _having = new LazyLoading<List<string>>(() => new List<string>());
            _joins = new LazyLoading<List<string>>(() => new List<string>());
            _select = new LazyLoading<List<string>>(() => new List<string>());
            _orderBy = new LazyLoading<List<string>>(() => new List<string>());
            _tableNames = new LazyLoading<List<string>>(() => new List<string>());
            Top = string.Empty;
        }

        internal string Top { get; set; }

        internal IList<string> GroupBy
        {
            get { return _groupBy.Value; }
        }

        internal IList<string> Having
        {
            get { return _having.Value; }
        }

        internal IList<string> Source
        {
            get { return _joins.Value; }
        }

        internal IList<string> Selection
        {
            get { return _select.Value; }
        }

        internal IList<string> OrderBy
        {
            get { return _orderBy.Value; }
        }

        internal IList<string> TableNames
        {
            get { return _tableNames.Value; }
        }
    }
}
