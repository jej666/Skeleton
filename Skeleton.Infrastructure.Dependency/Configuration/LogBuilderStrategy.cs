using Microsoft.Practices.ObjectBuilder2;
using Skeleton.Abstraction;
using Skeleton.Common;
using Skeleton.Infrastructure.Logging;

namespace Skeleton.Infrastructure.Dependency.Configuration
{
    public sealed class LogBuilderStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            context.ThrowIfNull(nameof(context));

            var policy = context.Policies.Get<ICreationStackTrackerPolicy>(null, true);

            if (policy.TypeStack.Count >= 2)
                if (policy.TypeStack.Peek(0) == typeof(ILogger))
                    context.Existing = new Logger(policy.TypeStack.Peek(1));

            base.PreBuildUp(context);
        }
    }
}