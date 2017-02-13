using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Skeleton.Common;
using System;

namespace Skeleton.Web.Server
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.ThrowIfNull(() => actionContext);

            if (!actionContext.ModelState.IsValid)
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);
        }
    }
}