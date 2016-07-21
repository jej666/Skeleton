using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal abstract class ContextBase
    {
        private readonly IDictionary<string, object> _parameters = new ExpandoObject();
        private readonly List<string> _conditions = new List<string>();
        private const string ParameterPrefix = "P";
        private int _paramIndex;

        protected internal string NextParamId()
        {
            ++_paramIndex;

            return ParameterPrefix +
                _paramIndex.ToString(CultureInfo.InvariantCulture);
        }

        protected internal IDictionary<string, object> Parameters
        {
            get { return _parameters; }
        }

        protected internal void AddParameter(string key, object value)
        {
            if (!Parameters.ContainsKey(key))
                Parameters.Add(key, value);
        }

        internal IList<string> Conditions
        {
            get { return _conditions; }
        }
    }
}
