using Skeleton.Core;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Skeleton.Web.Server.Configuration
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public virtual Task HandleAsync(
            ExceptionHandlerContext context,
            CancellationToken cancellationToken)
        {
            if (!ShouldHandle(context))
            {
                return Task.FromResult(0);
            }

            return HandleAsyncCore(context, cancellationToken);
        }

        public virtual Task HandleAsyncCore(
            ExceptionHandlerContext context,
            CancellationToken cancellationToken)
        {
            HandleCore(context);
            return Task.FromResult(0);
        }

        public virtual void HandleCore(ExceptionHandlerContext context)
        {
            context.ThrowIfNull(nameof(context));

            var content = Constants.DefaultErrorMessage;
#if DEBUG
            content = context.Exception.Message;
#endif

            context.Result = new TextPlainErrorResult
            {
                Request = context.ExceptionContext.Request,
                Content = content
            };
        }

        public virtual bool ShouldHandle(ExceptionHandlerContext context)
        {
            context.ThrowIfNull(nameof(context));

            return context.ExceptionContext.CatchBlock.IsTopLevel;
        }

        private class TextPlainErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public string Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(Content);
                response.RequestMessage = Request;

                return Task.FromResult(response);
            }
        }
    }
}