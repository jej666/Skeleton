//https://github.com/base33/lambda-sql-builder
namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    using Core.Repository;
    using System.Collections.Generic;

    public abstract class SqlBuilderBase : ISqlQuery, ISqlExecute
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

        protected SqlBuilderBase(string tableName)
        {
            _builder = new InternalQueryBuilder(tableName);
            _resolver = new LambdaExpressionResolver(_builder);
        }

        public string DeleteQuery
        {
            get { return _builder.Delete; }
        }

        public string InsertQuery
        {
            get { return _builder.Insert; }
        }

        public IDictionary<string, object> Parameters
        {
            get { return _builder.Parameters; }
        }

        public string Query
        {
            get { return _builder.Query; }
        }

        public string UpdateQuery
        {
            get { return _builder.Update; }
        }

        internal InternalQueryBuilder Builder { get { return _builder; } }

        internal LambdaExpressionResolver Resolver { get { return _resolver; } }

        public string PagedQuery(int pageSize, int pageNumber)
        {
            return _builder.PagedQuery(pageSize, pageNumber);
        }
    }
}