using System;
using System.Collections.Generic;

namespace Skeleton.Web.Client
{
    public static  class RestUriBuilderExtensions
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

        public static Uri Add(this IRestUriBuilder uriBuilder)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
                .StartNew()
                .AppendAction(RestAction.Add)
                .Uri;
        }

        public static Uri AddMany(this IRestUriBuilder uriBuilder)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
               .StartNew()
               .AppendAction(RestAction.AddMany)
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

        public static Uri UpdateMany(this IRestUriBuilder uriBuilder)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
             .StartNew()
             .AppendAction(RestAction.UpdateMany)
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

        public static Uri DeleteMany(this IRestUriBuilder uriBuilder)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            return uriBuilder
               .StartNew()
               .AppendAction(RestAction.DeleteMany)
               .Uri;
        }
    }
}
