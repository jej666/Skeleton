namespace Skeleton.Core.Repository
{
    using Common;
    using Core.Domain;
    using System.Collections.Generic;

    public interface IAggregateExecutor : IHideObjectMethods
    {
        TResult As<TResult>();
    }
}