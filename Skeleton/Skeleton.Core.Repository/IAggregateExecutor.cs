using Skeleton.Common;

namespace Skeleton.Core.Repository
{
    public interface IAggregateExecutor : IHideObjectMethods
    {
        TResult As<TResult>();
    }
}