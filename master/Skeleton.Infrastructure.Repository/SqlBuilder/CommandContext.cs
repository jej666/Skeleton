using System.Collections.Generic;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    public sealed class CommandContext : ContextBase
    {
        private readonly LazyRef<List<string>> _columns;
        private readonly LazyRef<List<string>> _columnValues;
        private readonly LazyRef<List<string>> _values;

        public CommandContext()
        {
            _columns = new LazyRef<List<string>>(() => new List<string>());
            _values = new LazyRef<List<string>>(() => new List<string>());
            _columnValues = new LazyRef<List<string>>(() => new List<string>());
        }

        public IList<string> Columns => _columns.Value;

        public IList<string> Values => _values.Value;

        public IList<string> ColumnValues => _columnValues.Value;
    }
}