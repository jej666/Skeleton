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
            context.ThrowIfNull();

            var content = Constants.DefaultErrorMessage;
#if DEBUG
            content = context.Exception.Message;
#endif
            var request = context.ExceptionContext.Request;
            var configuration = context.ExceptionContext.ControllerContext.Configuration;
            var negociator= configuration.Services.GetContentNegotiator();
            var negotiation = negociator.Negotiate(
                   typeof(string), request, configuration.Formatters);

            context.Result = new ErrorResult
            {
                Request = request,
                Content = new ObjectContent<string>(
                    content, 
                    negotiation.Formatter, 
                    negotiation.MediaType)
            };
        }

        public virtual bool ShouldHandle(ExceptionHandlerContext context)
        {
            context.ThrowIfNull();

            return context.ExceptionContext.CatchBlock.IsTopLevel;
        }

        private class ErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public ObjectContent Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {   
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                
                response.Content = Content;
                response.RequestMessage = Request;

                return Task.FromResult(response);
            }
        }
    }
}