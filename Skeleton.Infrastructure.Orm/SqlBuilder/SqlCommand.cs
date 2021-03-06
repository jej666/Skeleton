﻿using Skeleton.Abstraction.Data;
using System.Collections.Generic;

namespace Skeleton.Infrastructure.Orm.SqlBuilder
{
    public sealed class SqlCommand : ISqlCommand
    {
        internal SqlCommand(string sqlQuery, IDictionary<string, object> parameters)
        {
            SqlQuery = sqlQuery;
            Parameters = parameters;
        }

        public IDictionary<string, object> Parameters { get; private set; }

        public string SqlQuery { get; private set; }
    }
}