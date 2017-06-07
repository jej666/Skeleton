using System;

namespace Skeleton.Abstraction
{
    public interface ICacheConfiguration : IHideObjectMethods
    {
        void SetAbsoluteExpiration(DateTimeOffset absolute);

        void SetAbsoluteExpiration(TimeSpan relative);

        void SetSlidingExpiration(TimeSpan offset);
    }
}