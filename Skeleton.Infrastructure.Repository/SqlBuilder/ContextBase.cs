using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal abstract class ContextBase
    {
        private const string ParameterPrefix = "P";
        private readonly List<string> _conditions = new List<string>();
        private int _paramIndex;

        protected internal IDictionary<string, object> Parameters { get; } = new ExpandoObject();

        internal IList<string> Conditions => _conditions;

        protected internal string NextParamId()
        {
            ++_paramIndex;

            return ParameterPrefix +
                   _paramIndex.ToString(CultureInfo.InvariantCulture);
        }

        protected internal void AddParameter(string key, object value)
        {
            if (!Parameters.ContainsKey(key))
                Parameters.Add(key, value);
        }
    }
}