﻿namespace Skeleton.Infrastructure.DependencyResolver.LoggerExtension
{
    using Common;
    using Common.Extensions;
    using Logging;
    using Microsoft.Practices.ObjectBuilder2;

    public class LogBuilderStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            context.ThrowIfNull(() => context);

            var policy = context.Policies.Get<ICreationStackTrackerPolicy>(null, true);

            if (policy.TypeStack.Count >= 2)
            {
                if (policy.TypeStack.Peek(0) == typeof(ILogger))
                {
                    context.Existing = LoggerFactory.GetLogger(policy.TypeStack.Peek(1));
                }
            }

            base.PreBuildUp(context);
        }
    }
}