using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    public abstract class ContextBase
    {
        private const string ParameterPrefix = "P";
        private readonly List<string> _conditions = new List<string>();
        private int _paramIndex;

        public IDictionary<string, object> Parameters { get; } = new ExpandoObject();

        public IList<string> Conditions => _conditions;

        public string NextParamId()
        {
            ++_paramIndex;

            return ParameterPrefix +
                   _paramIndex.ToString(CultureInfo.InvariantCulture);
        }

        public void AddParameter(string key, object value)
        {
            if (!Parameters.ContainsKey(key))
                Parameters.Add(key, value);
        }
    }
}