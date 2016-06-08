using Skeleton.Common;
using Skeleton.Core.Service;
using System;

namespace Skeleton.Infrastructure.Service
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IDependencyResolver _resolver;

        public QueryDispatcher(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public TResult Execute<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var handler = _resolver.Resolve<IQueryHandler<TQuery, TResult>>();

            if (handler == null)
            {
                throw new ArgumentNullException(typeof(TQuery).Name);
            }

            return handler.Execute(query);
        }
    }
}
