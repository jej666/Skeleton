using Skeleton.Common;
using System.Collections.Generic;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class CommandContext : ContextBase
    {
        private readonly LazyRef<List<string>> _columns;
        private readonly LazyRef<List<string>> _columnValues;
        private readonly LazyRef<List<string>> _values;

        internal CommandContext()
        {
            _columns = new LazyRef<List<string>>(() => new List<string>());
            _values = new LazyRef<List<string>>(() => new List<string>());
            _columnValues = new LazyRef<List<string>>(() => new List<string>());
        }

        internal IList<string> Columns => _columns.Value;

        internal IList<string> Values => _values.Value;

        internal IList<string> ColumnValues => _columnValues.Value;
    }
}