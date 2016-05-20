//https://github.com/base33/lambda-sql-builder

using System.Collections.Generic;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    public class SqlBuilderBase : ISqlQuery, ISqlExecute
    {
        private readonly InternalQueryBuilder _builder;
        private readonly LambdaExpressionResolver _resolver;

        internal SqlBuilderBase(
            InternalQueryBuilder builder,
            LambdaExpressionResolver resolver)
        {
            _builder = builder;
            _resolver = resolver;
        }

        internal SqlBuilderBase(string tableName)
        {
            _builder = new InternalQueryBuilder(tableName);
            _resolver = new LambdaExpressionResolver(_builder);
        }

        internal InternalQueryBuilder Builder
        {
            get { return _builder; }
        }

        internal LambdaExpressionResolver Resolver
        {
            get { return _resolver; }
        }

        public string DeleteQuery
        {
            get { return _builder.Delete; }
        }

        public string InsertQuery
        {
            get { return _builder.Insert; }
        }

        public string UpdateQuery
        {
            get { return _builder.Update; }
        }

        public IDictionary<string, object> Parameters
        {
            get { return _builder.Parameters; }
        }

        public string Query
        {
            get { return _builder.Query; }
        }

        public string PagedQuery(int pageSize, int pageNumber)
        {
            return _builder.PagedQuery(pageSize, pageNumber);
        }
    }
}