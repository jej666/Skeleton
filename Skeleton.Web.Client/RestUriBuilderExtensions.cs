using System;
using System.Collections.Generic;

namespace Skeleton.Web.Client
{
    public static class RestUriBuilderExtensions
    {
        public static Uri GetAll(this IRestUriBuilder uriBuilder)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
               .StartNew()
               .AppendAction(RestAction.GetAll)
               .Uri;
        }

        public static Uri FirstOrDefault(this IRestUriBuilder uriBuilder, object id)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
               .StartNew()
               .AppendAction(RestAction.Get)
               .AppendAction(id)
               .Uri;
        }

        public static Uri Page(this IRestUriBuilder uriBuilder, int pageSize, int pageNumber)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
               .StartNew()
               .AppendAction(RestAction.Page)
               .SetQueryParameters(new Dictionary<string, object>
               {
                    { "PageSize", pageSize },
                    { "pageNumber", pageNumber }
               })
               .Uri;
        }

        public static Uri Create(this IRestUriBuilder uriBuilder)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
                .StartNew()
                .AppendAction(RestAction.Create)
                .Uri;
        }

        public static Uri BatchCreate(this IRestUriBuilder uriBuilder)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
               .StartNew()
               .AppendAction(RestAction.BatchCreate)
               .Uri;
        }

        public static Uri Update(this IRestUriBuilder uriBuilder)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
             .StartNew()
             .AppendAction(RestAction.Update)
             .Uri;
        }

        public static Uri BatchUpdate(this IRestUriBuilder uriBuilder)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
             .StartNew()
             .AppendAction(RestAction.BatchUpdate)
             .Uri;
        }

        public static Uri Delete(this IRestUriBuilder uriBuilder, object id)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
              .StartNew()
              .AppendAction(RestAction.Delete)
              .AppendAction(id)
              .Uri;
        }

        public static Uri BatchDelete(this IRestUriBuilder uriBuilder)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
               .StartNew()
               .AppendAction(RestAction.BatchDelete)
               .Uri;
        }
    }
}