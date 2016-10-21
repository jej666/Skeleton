using System.Collections.Generic;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    public sealed class QueryContext : ContextBase
    {
        private readonly LazyRef<List<string>> _groupBy;
        private readonly LazyRef<List<string>> _having;
        private readonly LazyRef<List<string>> _joins;
        private readonly LazyRef<List<string>> _orderBy;
        private readonly LazyRef<List<string>> _select;
        private readonly LazyRef<List<string>> _tableNames;

        public QueryContext()
        {
            _groupBy = new LazyRef<List<string>>(() => new List<string>());
            _having = new LazyRef<List<string>>(() => new List<string>());
            _joins = new LazyRef<List<string>>(() => new List<string>());
            _select = new LazyRef<List<string>>(() => new List<string>());
            _orderBy = new LazyRef<List<string>>(() => new List<string>());
            _tableNames = new LazyRef<List<string>>(() => new List<string>());
            Top = string.Empty;
        }

        public string Top { get; set; }

        public IList<string> GroupBy => _groupBy.Value;

        public IList<string> Having => _having.Value;

        public IList<string> Source => _joins.Value;

        public IList<string> Selection => _select.Value;

        public IList<string> OrderBy => _orderBy.Value;

        public IList<string> TableNames => _tableNames.Value;
    }
}