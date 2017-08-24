using Skeleton.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Skeleton.Web.Server.Configuration
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CheckModelForNullAttribute : ActionFilterAttribute
    {
        private const string Error = "The argument cannot be null";
        private readonly Func<Dictionary<string, object>, bool> _checkCondition;

        public CheckModelForNullAttribute()
            : this(arguments => arguments.ContainsValue(null))
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public CheckModelForNullAttribute(Func<Dictionary<string, object>, bool> checkCondition)
        {
            _checkCondition = checkCondition;
        }

        public Func<Dictionary<string, object>, bool> CheckCondition
        {
            get { return _checkCondition; }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.ThrowIfNull();

            if (CheckCondition(actionContext.ActionArguments))
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, Error);
        }
    }
}