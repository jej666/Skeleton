using System.Collections.Generic;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class CommandContext : ContextBase
    {
        private readonly LazyLoading<List<string>> _columns;
        private readonly LazyLoading<List<string>> _values;
        private readonly LazyLoading<List<string>> _columnValues;

        internal CommandContext()
        {
            _columns = new LazyLoading<List<string>>(() => new List<string>());
            _values = new LazyLoading<List<string>>(() => new List<string>());
            _columnValues = new LazyLoading<List<string>>(() => new List<string>());
        }

        internal IList<string> Columns
        {
            get { return _columns.Value; }
        }

        internal IList<string> Values
        {
            get { return _values.Value; }
        }

        internal IList<string> ColumnValues
        {
            get { return _columnValues.Value; }
        }
    }
}
