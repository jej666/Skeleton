using System.Collections.Generic;
using Skeleton.Shared.CommonTypes;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class QueryContext : ContextBase
    {
        private readonly LazyRef<List<string>> _groupBy;
        private readonly LazyRef<List<string>> _having;
        private readonly LazyRef<List<string>> _joins;
        private readonly LazyRef<List<string>> _orderBy;
        private readonly LazyRef<List<string>> _select;
        private readonly LazyRef<List<string>> _tableNames;

        internal QueryContext()
        {
            _groupBy = new LazyRef<List<string>>(() => new List<string>());
            _having = new LazyRef<List<string>>(() => new List<string>());
            _joins = new LazyRef<List<string>>(() => new List<string>());
            _select = new LazyRef<List<string>>(() => new List<string>());
            _orderBy = new LazyRef<List<string>>(() => new List<string>());
            _tableNames = new LazyRef<List<string>>(() => new List<string>());
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